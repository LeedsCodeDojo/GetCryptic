package io.github.leedscodedojo;

import java.util.ArrayList;
import java.util.List;

public class Xor {
    public List<Byte> execute(List<Byte> sourceBytes, List<Byte> key) {
        if (sourceBytes.size() != key.size()){
            throw new IllegalArgumentException("Sizes are different");
        }

        List<Byte> encoded = new ArrayList<>();

        for(int i = 0 ; i < sourceBytes.size() ; i++){
            int i1 = sourceBytes.get(i) ^ key.get(i);
            encoded.add((byte)i1);
        }

        return encoded;
    }
}
