package io.github.leedscodedojo;

public class DecodedResult {
    @Override
    public String toString() {
        return "DecodedResullt{" +
                "score=" + score +
                ", decodedText='" + decodedText + '\'' +
                ", key=" + key +
                '}';
    }

    public long score;
    public String decodedText;
    public byte key;

    public DecodedResult(long score, String decodedText, byte key) {
        this.score = score;
        this.decodedText = decodedText;
        this.key = key;
    }
}


