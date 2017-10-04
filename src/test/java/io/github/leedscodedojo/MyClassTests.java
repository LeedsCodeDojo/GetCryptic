package io.github.leedscodedojo;

import org.junit.Test;

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
}
