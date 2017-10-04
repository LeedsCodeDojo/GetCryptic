package io.github.leedscodedojo;

import com.sun.java.swing.plaf.windows.WindowsTreeUI;
import org.junit.Test;

import java.util.Arrays;
import java.util.Collections;
import java.util.Dictionary;
import java.util.List;
import java.util.stream.Collectors;

import static org.hamcrest.CoreMatchers.is;
import static org.junit.Assert.assertThat;

public class MyClassTests {
    @Test
    public void testEncode() {
        String source = "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d";
        String expected = "SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t";

        Base64Encoder encoder = new Base64Encoder();

        String result = encoder.encode(source);

        assertThat(result, is(expected));
    }

    @Test
    public void testEncodeThreeBytes() {
        String source = "49276d";
        String expected = "SSdt";

        Base64Encoder encoder = new Base64Encoder();

        String result = encoder.encode(source);

        assertThat(result, is(expected));
    }

    @Test
    public void testEncodeWithPadding() {
        String source = "4927";
        String expected = "SSc=";

        Base64Encoder encoder = new Base64Encoder();

        String result = encoder.encode(source);

        assertThat(result, is(expected));
    }

    @Test
    public void testEncodeWithPaddings() {
        String source = "49";
        String expected = "SQ==";

        Base64Encoder encoder = new Base64Encoder();

        String result = encoder.encode(source);

        assertThat(result, is(expected));
    }

    @Test
    public void hexToByteTest1() {
        char char1 = '0';
        char char2 = '0';

        assertThat(new Base64Encoder().charsToByte(char1, char2), is((byte)0));
    }

    @Test
    public void hexToByteTest2() {
        char char1 = '1';
        char char2 = '1';

        assertThat(new Base64Encoder().charsToByte(char1, char2), is((byte)17));
    }

    @Test
    public void hexToByteTest3() {
        char char1 = 'a';
        char char2 = '1';

        assertThat(new Base64Encoder().charsToByte(char1, char2), is((byte)161));
    }

    @Test
    public void lookupBase64Test1() {
        assertThat(new Base64Encoder().lookupBase64(0), is('A'));
    }

    @Test
    public void lookupBase64Test2() {
        assertThat(new Base64Encoder().lookupBase64(51), is('z'));
    }

    @Test
    public void lookupBase64Test3() {
        assertThat(new Base64Encoder().lookupBase64(61), is('9'));
    }

    @Test
    public void lookupBase64Test4() {
        assertThat(new Base64Encoder().lookupBase64(62), is('+'));
    }

    @Test
    public void lookupBase64Test5() {
        assertThat(new Base64Encoder().lookupBase64(63), is('/'));
    }

    @Test
    public void fixedXOr(){
        List<Byte> sourceBytes = new Base64Encoder().hexDecode("1c0111001f010100061a024b53535009181c");
        List<Byte> key = new Base64Encoder().hexDecode("686974207468652062756c6c277320657965");
        List<Byte> expected = new Base64Encoder().hexDecode("746865206b696420646f6e277420706c6179");

         List<Byte> actual =  new Xor().execute(sourceBytes, key);
         assertThat(actual, is(expected));
    }

    @Test
    public void decode(){
        List<Byte> cipherText = new Base64Encoder().hexDecode("1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736");

        //Cooking MC's like a pound of bacon
        List<Byte> commonCharacters = Arrays.asList(new Byte[]{'a', 'e', 'i', 'o', 'r', 's'});

        Long maxScore = -1L;
        byte bestKey = Byte.MIN_VALUE;
        String bestDecoded = "";

        for(byte key = Byte.MIN_VALUE ; key < Byte.MAX_VALUE; key ++){
            Byte[] keyBytes = new Byte[cipherText.size()];
            Arrays.fill(keyBytes, key);

            List<Byte> decoded = new Xor().execute(cipherText, Arrays.asList(keyBytes));

            long score = decoded
                    .stream()
                    .filter(decodedChar -> commonCharacters.contains(decodedChar))
                    .count();


            String decodedText = decoded.stream()
                    .map(b -> Character.valueOf((char) (int) b))
                    .map(c -> c.toString())
                    .collect(Collectors.joining());

            System.out.println(score);
            System.out.println(key);
            System.out.println(decodedText);

            if (score > maxScore){
                maxScore = score;
                bestKey = key;
                bestDecoded = decodedText;
            }
        }

        System.out.println("And the winner is...");
        System.out.println(bestKey);
        System.out.println(bestDecoded);


        //88
        //Cooking MC's like a pound of bacon
    }
}
