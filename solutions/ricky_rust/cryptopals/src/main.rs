pub mod cryptopals;
use cryptopals::*;

fn set_1_challenge_1() {
    let hex = "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d";
    
    //splitting up long string to byte hex
    let hex_vec = string_to_hex_vec(hex.to_string());

    //converting to long binary vector
    let mut bin_string_vec: Vec<Vec<String>> = Vec::new();
    for i in hex_vec.iter() {
        bin_string_vec.push(num_to_bin_vec_vec(i));
    }

    //unwrapping Vec<Vec<>>
    let bin_vec = vec_vec_fix(bin_string_vec); 

    //outputting binary words
    let bin_base: Vec<String> = bin_to_new_base(&bin_vec, 6);

    //converting to numbers for base64 format
    let base_num: Vec<u8> = bin_string_to_num(&bin_base);

    //outputting base64 :) 
    println!("{:?}", to_base64(&base_num)); 
}

fn set_1_challenge_2() {
    let values = "1c0111001f010100061a024b53535009181c";
    let mask = "686974207468652062756c6c277320657965";
    let mut masked_value: Vec<u8> = Vec::new();

    let hexed_values = string_to_hex_vec(values.to_string());
    let hexed_mask = string_to_hex_vec(mask.to_string());

    for xy in hexed_values.iter().zip(hexed_mask) {
        let (x, y) = xy;
        masked_value.push(x^y);
    }

    let string_masked_values = hex_vec_to_string(masked_value);

    println!("{}", string_masked_values); 
}

fn set_1_challenge_3() {
    let values = "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736";
    let hex_values = string_to_hex_vec(values.to_string());
    let mut masked_values_string_vec: Vec<String> = Vec::new();
    let mut best_result = String::new();
    let mut score;
    let mut best_score = 0;

    for m in 0..255 {
        let mut masked_values: Vec<u8> = Vec::new();
        for i in hex_values.iter() {
            masked_values.push(i^m);
        }
        masked_values_string_vec.push(to_string(masked_values));
    }

    for s in masked_values_string_vec.iter() {
        score = challenge_3_score(&s);

        if score > best_score {
            best_result = s.to_string();
            best_score = score
        }
    }

    //getting the index of the correct string which will be the mask:
    let index = masked_values_string_vec.iter().enumerate().find(|r| r.1 == &best_result).unwrap();
    
    println!("mask 0x{:X}: phrase: {}", index.0, best_result);
}

fn main() {
    //Run challenge 1
    set_1_challenge_1();

    //Run challenge 2
    set_1_challenge_2();

    //Run challenge 3
    set_1_challenge_3();
}