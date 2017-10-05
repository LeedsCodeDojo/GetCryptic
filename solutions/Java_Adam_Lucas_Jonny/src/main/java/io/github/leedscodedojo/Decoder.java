package io.github.leedscodedojo;

import java.util.Arrays;
import java.util.List;
import java.util.stream.Collectors;

public class Decoder {

    public static DecodedResult decodeCipherText(String cipherTextString) {
        List<Byte> cipherText = new Base64Encoder().hexDecode(cipherTextString);

        List<Byte> commonCharacters = "abcdefghijklmnopqrstuvwxyz ".chars()
                .mapToObj(i -> (byte) i)
                .collect(Collectors.toList());

        DecodedResult bestResult = null;

        for (byte key = Byte.MIN_VALUE; key < Byte.MAX_VALUE; key++) {
            Byte[] keyBytes = new Byte[cipherText.size()];
            Arrays.fill(keyBytes, key);

            List<Byte> decoded = new Xor().execute(cipherText, Arrays.asList(keyBytes));

            long score = decoded
                    .stream()
                    .filter(commonCharacters::contains)
                    .count();

            String decodedText = decoded.stream()
                    .map(b -> ((char) (int) b))
                    .map(Object::toString)
                    .collect(Collectors.joining());

            if (bestResult == null || bestResult.score < score) {
                bestResult = new DecodedResult(score, decodedText, key);
            }
        }

        return bestResult;
    }
}
