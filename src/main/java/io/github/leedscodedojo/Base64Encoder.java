package io.github.leedscodedojo;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

/**
 * Created by Adam on 04/10/2017.
 */
public class Base64Encoder {

    private static final List<Character> HEX_CHARS = Arrays.asList(new Character[]{'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'});

    public String encode(String source) {
        List<Byte> sourceToBytes = new ArrayList<>();

        char[] sourceChars = source.toLowerCase().toCharArray();

        for (int i=0; i<sourceChars.length-1; i+=2) {
            char char1 = sourceChars[i];
            char char2 = sourceChars[i+1];

            sourceToBytes.add(charsToByte(char1, char2));
        }

        StringBuilder result = new StringBuilder();

        for (int i=0; i<sourceToBytes.size(); i+=3) {
            if (sourceToBytes.size() - i < 3) {
                switch (sourceToBytes.size() - i) {
                    case 1:
                        int bits24a = (sourceToBytes.get(i) << 16);

                        byte byteOut1a = (byte)((bits24a >> 18) & 0x3f);
                        byte byteOut2a = (byte)((bits24a >> 12) & 0x3f);

                        result.append(lookupBase64(byteOut1a));
                        result.append(lookupBase64(byteOut2a));
                        result.append('=');
                        result.append('=');
                        break;
                    case 2:
                        int bits24b = (sourceToBytes.get(i) << 16) | (sourceToBytes.get(i+1) << 8);

                        byte byteOut1b = (byte)((bits24b >> 18) & 0x3f);
                        byte byteOut2b = (byte)((bits24b >> 12) & 0x3f);
                        byte byteOut3b = (byte)((bits24b >> 6) & 0x3f);

                        result.append(lookupBase64(byteOut1b));
                        result.append(lookupBase64(byteOut2b));
                        result.append(lookupBase64(byteOut3b));
                        result.append('=');
                        break;
                    default:
                        throw new IllegalStateException("What?");
                }
                break;
            }

            int bits24 = (sourceToBytes.get(i) << 16) | (sourceToBytes.get(i+1) << 8) | sourceToBytes.get(i+2);

            byte byteOut1 = (byte)((bits24 >> 18) & 0x3f);
            byte byteOut2 = (byte)((bits24 >> 12) & 0x3f);
            byte byteOut3 = (byte)((bits24 >> 6) & 0x3f);
            byte byteOut4 = (byte)(bits24 & 0x3f);

            result.append(lookupBase64(byteOut1));
            result.append(lookupBase64(byteOut2));
            result.append(lookupBase64(byteOut3));
            result.append(lookupBase64(byteOut4));
        }

        return result.toString();
    }

    public byte charsToByte(char c1, char c2) {
        int value = HEX_CHARS.indexOf(c1);
        int value2 = HEX_CHARS.indexOf(c2);
        return (byte)(((value & 0xf) << 4) | (value2 & 0xf));
    }

    public char lookupBase64(int value) {
        if (value < 26) {
            return (char)((int)'A' + value);
        } else if (value < 52) {
            return (char)((int)'a' + value - 26);
        } else if (value < 62) {
            return (char)((int)'0' + value - 52);
        } else if (value == 62) {
            return '+';
        } else if (value == 63) {
            return '/';
        } else {
            throw new IllegalStateException("Unexpected value " + value);
        }
    }
}
