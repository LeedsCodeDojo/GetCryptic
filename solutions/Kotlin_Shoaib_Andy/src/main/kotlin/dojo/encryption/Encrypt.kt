package dojo.encryption

import kotlin.streams.toList

val hexCodes = listOf('0','1', '2', '3', '4','5', '6', '7', '8', '9','a', 'b','c', 'd','e', 'f')

fun Int.toHexChar() = hexCodes[this]

fun Char.hexCharToDigit() = hexCodes.indexOf(this)

fun generateRepeatingHexSequence(asciiCode: Int, repeat: Int): Sequence<Char>{
    val lastChar = (asciiCode and 15).toHexChar()
    val firstChar = ((asciiCode shr 4) and 15).toHexChar()
    return ("" + firstChar + lastChar).repeat(repeat).asSequence()
}

fun hexToBase64(hex: String) : String {
    return hexToInt24(hex).joinToString("") { int24ToBase64(it) }
}

fun hexToInts(hex: Sequence<Char>): Sequence<Int>{
    val evenChars = hex.filterIndexed { index, _ -> index % 2 == 0 }
    val oddChars = hex.filterIndexed { index, _ -> index % 2 == 1 }
    return evenChars.zip(oddChars).map { hexPairsToByte(it.first, it.second) }
}

fun hexToChars(hex: Sequence<Char>) : Sequence<Char> {
    return hexToInts(hex).map { it.toChar() }
}

fun hexPairsToByte(c1: Char, c2: Char) : Int {
    return (c1.hexCharToDigit() shl 4) or c2.hexCharToDigit()
}



fun hexToInt24(hex: String): List<Int>{
    val int24s = mutableListOf<Int>()
    val chars = hex.iterator()
    while (chars.hasNext()){
        var int24 = chars.nextChar().hexCharToDigit()
        (1..5).forEach {
            int24 = int24 shl 4 or chars.nextChar().hexCharToDigit()
        }
        int24s.add(int24)
    }
    return int24s
}

fun int24ToBase64(int24: Int) : String {
    return (3 downTo 0)
            .map { int24 shr (it*6) }
            .map { int6ToChar(it and 63) }
            .joinToString("")
}

fun int6ToChar(int6: Int): Char {
    return when {
        int6 < 26 -> ('A'.toInt() + int6).toChar()
        int6 < 52 -> ('a'.toInt() + int6 - 26).toChar()
        int6 < 62 -> ('0'.toInt() + int6 - 52).toChar()
        int6 == 62 -> '+'
        int6 == 63 -> '/'
        else -> throw IllegalArgumentException("Invalid number")
    }
}

// Count the number of valid Letters, Digits & whitespace in a character sequence.  Basic but it works :).
private fun Sequence<Char>.score() = this.count { c -> c.isLetterOrDigit() || c.isWhitespace() }

fun Sequence<Char>.xorHex(hex2: Sequence<Char>) : Sequence<Char> {
    return this.zip(hex2)
            .map { (c1, c2) -> c1.hexCharToDigit() xor c2.hexCharToDigit() }
            .map { it.toHexChar() }
}

fun String.singleByteXor() : Pair<String,Int> {
    val byteCount = this.length / 2
    val hexSequence: Sequence<Char> = this.asSequence()
    val bestString = (1..255)
            .map { hexSequence.xorHex(generateRepeatingHexSequence(it, byteCount))}
            .map { hexToChars(it) }
            .maxBy { it.score() }!!
    return Pair(bestString.joinToString(""), bestString.score())
}

fun findBestMatch(lines : List<String>): Pair<String, Int>{
    return lines.parallelStream()
            .map { it.singleByteXor() }
            .toList()
            .maxBy { it.second }!!
}

