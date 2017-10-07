package dojo.encryption

import org.hamcrest.MatcherAssert.assertThat
import org.hamcrest.core.IsEqual.equalTo
import org.hamcrest.core.StringStartsWith.startsWith
import org.junit.Test


class EncryptTest {

    @Test
    fun `check that cryptopals test produces expected result`() {
        assertThat(hexToBase64("49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d"),
                equalTo("SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t"))
    }

    @Test
    fun `check hexToDigit`() {
        assertThat('0'.hexCharToDigit(), equalTo(0))
        assertThat('2'.hexCharToDigit(), equalTo(2))
        assertThat('5'.hexCharToDigit(), equalTo(5))
        assertThat('9'.hexCharToDigit(), equalTo(9))
        assertThat('a'.hexCharToDigit(), equalTo(10))
        assertThat('c'.hexCharToDigit(), equalTo(12))
        assertThat('f'.hexCharToDigit(), equalTo(15))
    }

    @Test
    fun `process pairs of chars`() {
        assertThat(hexPairsToByte('4', '9'), equalTo(73))
    }

    @Test fun `generate list of integers from hex`(){
        assertThat(hexToInts("49276d".asSequence()).toList(), equalTo(listOf(73,39,109)))
    }

    @Test fun `generate nibbles of integers from hex`(){
        assertThat(hexToInt24("49276d"), equalTo(listOf(4794221)))
    }

    @Test fun `int6 to Char`(){
        assertThat(int6ToChar(62) == '+', equalTo(true))
        assertThat(int6ToChar(63) == '/', equalTo(true))
        assertThat(int6ToChar(1) == 'B', equalTo(true))
        assertThat(int6ToChar(30) == 'e', equalTo(true))
        assertThat(int6ToChar(53) == '1', equalTo(true))
    }

    @Test fun `int24 to Chars`(){
        val int24s = hexToInt24("49276d")
        assertThat(int24s.size, equalTo(1))
        assertThat(int24ToBase64(int24s[0]), equalTo("SSdt"))
    }

    @Test fun `xorHex`(){
        assertThat("1c0111001f010100061a024b53535009181c".asSequence()
                    .xorHex("686974207468652062756c6c277320657965".asSequence())
                .joinToString(""),
                equalTo("746865206b696420646f6e277420706c6179"))
    }

    @Test fun `generate repeating hex sequence`(){
        assertThat(generateRepeatingHexSequence(5, 6).joinToString(""),
                equalTo("050505050505"))
        assertThat(generateRepeatingHexSequence(25, 8).joinToString(""),
                equalTo("1919191919191919"))
        assertThat(generateRepeatingHexSequence(161, 3).joinToString(""),
                equalTo("a1a1a1"))
        assertThat(generateRepeatingHexSequence(255, 3).joinToString(""),
                equalTo("ffffff"))
    }

    @Test fun `single Xor Operation`(){
        val bestMatch = singleByteXor("1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736")
        assertThat(bestMatch.first, equalTo("Cooking MC's like a pound of bacon"))
        assertThat(bestMatch.second, equalTo(33))
    }

    @Test fun `findBestMatchInList`(){
        val inputStream = this.javaClass.classLoader.getResourceAsStream("set1/findXor.txt")
        val bestMatch = findBestMatch(inputStream.bufferedReader().readLines())

        assertThat(bestMatch.first, startsWith("Now that the party is jumping"))
        assertThat(bestMatch.second, equalTo(30))
    }

}