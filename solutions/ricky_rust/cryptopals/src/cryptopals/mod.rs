use std::collections::HashMap;

//safely convert from a string to hex using built in function
pub fn to_hex(input: &str) -> Option<u8> {
    let output;
    
    if input.len() == 2 { //one byte
        output = match u8::from_str_radix(input, 16) { //outputting this safely 
               Ok(e) => Some(e), 
               Err(_) => None,
            }
        ;
    }
    else if input.len() == 4 { //one byte with 0x infront
        output = match u8::from_str_radix(&input[2..], 16) {
                Ok(e) => Some(e),
                Err(_) => None,
            };
    }
    else {
        panic!("I can't convert {} to hex!", input); //fail and die, output what we failed to convert
    }

    output
}

//transforms a vec of hex to a string (utf8)
pub fn to_string(input: Vec<u8>) -> String {
    String::from_utf8_lossy(&input).to_string()
}

//iterate over a long string and move the whole thing to hex
pub fn string_to_hex_vec(input: String) -> Vec<u8> {
    let mut output: Vec<u8> = Vec::new();
    let mut i = 0;

    while i+1 < input.len() {
        let hex_string;

        if input.len()%2 != 0 { //assume first byte is missing leading 0
            if &input[i..i+2] == "0x" || &input[i..i+2] == "0X" { //checking for 0x
                hex_string = format!("{}{}{}", input[i..i+2].to_string(), "0".to_string(),
                input[i+2..i+3].to_string());
                output.push(to_hex(&hex_string).unwrap());
                i+=3;
            }
            else { //if no 0x but odd assume 0x0Z needs to take place
                hex_string = format!("{}{}", "0".to_string(), input[..i+1].to_string());
                output.push(to_hex(&hex_string).unwrap());
                i+=1;
            }
        }

        else { //not odd move on! 
            if &input[i..i+2] == "0x" || &input[i..i+2] == "0X" { //checking for 0x
                hex_string = format!("{}", input[i..i+4].to_string());
                output.push(to_hex(&hex_string).unwrap());
                i+=4;
            }
            else {
                hex_string = format!("{}", input[i..i+2].to_string()); //no 0x just grab the first two 
                output.push(to_hex(&hex_string).unwrap());
                i+=2;
            }
        }
    }

    output
}

pub fn hex_to_string(input: u8) -> String { //convert from a value to the string representation of the hex
    let  output;
    output = format!("{:X}", input);

    output
}

pub fn hex_vec_to_string(input: Vec<u8>) -> String { //convert a lot of hex to a long string representation of the hex
    let mut output = String::new();

    for hex in input.iter() {
        output += &format!("{:X}", hex);
    }

    output
}

pub fn num_to_bin_vec_vec(input: &u8) -> Vec<String> {
    let mut output: Vec<String> = Vec::new();
    let masks = vec!(0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01); //masks to check the bits at each location
    
        for i in masks.iter() {
            if input&i == 0x00 { //if the result is zero there is a 0 in the space
                output.push("0".to_string());
            }
            else { //in this case there's a 1 in them thar' hills 
                output.push("1".to_string());
            }
        }

    output 
}

//unwraps a vec<vec<>> to just vec<>
//this helps us when we need to format to a difference base
pub fn vec_vec_fix(input: Vec<Vec<String>>) -> Vec<String> {
    let mut output: Vec<String> = Vec::new();

    for i in input.iter() {
        for b in i.iter() {
            output.push(b.to_owned());
        }
    }

    output //outputs a Vec<String> of all "1"s and "0"s 
}

pub fn bin_to_new_base(input: &Vec<String>, base: usize) -> Vec<String> {
    let bin_num; //how many numbers will result from the set
    let mut bin_num_i = 0; //for number of bytes to output
    let mut bit_i = 0; //for number of bits per number
    let mut index_num = 0; //for index of input array

    let mut output: Vec<String> = Vec::new();

    //how many resulting numbers from the vec of bits there should be
    //helps with padding
    if input.len()%base == 0 {
        bin_num = input.len()/base;
    }
    else {
        bin_num = input.len()%base;
    }

    while bin_num_i < bin_num {  
        let mut bin_string = String::new();
        while bit_i < base {
            if index_num < input.len() {
                bin_string += &format!("{}", input[index_num]); //adding until we get to the proper base
            }
            else {
                bin_string += &format!("0"); //padding to the right
            }
            bit_i += 1;
            index_num += 1;
        }
        bin_num_i += 1;
        bit_i = 0;
        output.push(bin_string.to_string()); //push each number to the output vector
    }

    output    
}

pub fn bin_string_to_num(input: &Vec<String>) -> Vec<u8> {
    let mut output: Vec<u8> = Vec::new();

    for i in input.iter() {
        output.push(u8::from_str_radix(i, 2).unwrap()); //converts string of binary to u8 (left pads)
    }

    output
}


//value to hex acii value
pub fn to_base64(input: &Vec<u8>) -> String {
    let mut output = String::new();

    /* We ignore padding in this example
    let mut padding = 0;
    if input.len()%3 != 0 {
        padding = 3-input.len()%3;
    }
    */

    //large hashmap to map hex to base64 chacters
    let mut base64_mapping: HashMap<u8, u8> = HashMap::new();
        base64_mapping.insert(0, 65);
        base64_mapping.insert(1, 66);
        base64_mapping.insert(2, 67);
        base64_mapping.insert(3, 68);
        base64_mapping.insert(4, 69);
        base64_mapping.insert(5, 70);
        base64_mapping.insert(6, 71);
        base64_mapping.insert(7, 72);
        base64_mapping.insert(8, 73);
        base64_mapping.insert(9, 74);
        base64_mapping.insert(10, 75);
        base64_mapping.insert(11, 76);
        base64_mapping.insert(12, 77);
        base64_mapping.insert(13, 78);
        base64_mapping.insert(14, 79);
        base64_mapping.insert(15, 80);
        base64_mapping.insert(16, 81);
        base64_mapping.insert(17, 82);
        base64_mapping.insert(18, 83);
        base64_mapping.insert(19, 84);
        base64_mapping.insert(20, 85);
        base64_mapping.insert(21, 86);
        base64_mapping.insert(22, 87);
        base64_mapping.insert(23, 88);
        base64_mapping.insert(24, 89);
        base64_mapping.insert(25, 90);
        base64_mapping.insert(26, 97);
        base64_mapping.insert(27, 98);
        base64_mapping.insert(28, 99);
        base64_mapping.insert(29, 100);
        base64_mapping.insert(30, 101);
        base64_mapping.insert(31, 102);
        base64_mapping.insert(32, 103);
        base64_mapping.insert(33, 104);
        base64_mapping.insert(34, 105);
        base64_mapping.insert(35, 106);
        base64_mapping.insert(36, 107);
        base64_mapping.insert(37, 108);
        base64_mapping.insert(38, 109);
        base64_mapping.insert(39, 110);
        base64_mapping.insert(40, 111);
        base64_mapping.insert(41, 112);
        base64_mapping.insert(42, 113);
        base64_mapping.insert(43, 114);
        base64_mapping.insert(44, 115);
        base64_mapping.insert(45, 116);
        base64_mapping.insert(46, 117);
        base64_mapping.insert(47, 118);
        base64_mapping.insert(48, 119);
        base64_mapping.insert(49, 120);
        base64_mapping.insert(50, 121);
        base64_mapping.insert(51, 122);
        base64_mapping.insert(52, 48);
        base64_mapping.insert(53, 49);
        base64_mapping.insert(54, 50);
        base64_mapping.insert(55, 51);
        base64_mapping.insert(56, 52);
        base64_mapping.insert(57, 53);
        base64_mapping.insert(58, 54);
        base64_mapping.insert(59, 55);
        base64_mapping.insert(60, 56);
        base64_mapping.insert(61, 57);
        base64_mapping.insert(62, 43);
        base64_mapping.insert(63, 47);

    for i in input.iter() {
            output += &format!("{}", String::from_utf8(vec!(*base64_mapping.get(i) 
            .expect(&format!("FAIL, the hex was outside the base64 range 0-63: {}", i)))
            ).unwrap()); //for each number in the converted array and load in the new character
    }

/* Again ingore padding
    let mut i = 0;
    while i < padding {
        output += "=";
        i+=1;
    }
*/
    output
}

pub fn challenge_3_score(input: &String) -> usize {
    let mut score = 0;
    
    //creating a hash for "ETAOIN SHRDLU"
    //most popular chacters 
    let mut score_mapping: HashMap<String, usize> = HashMap::new();
       score_mapping.insert("e".to_string(), 2);
       score_mapping.insert("E".to_string(), 1);
       score_mapping.insert("t".to_string(), 2);
       score_mapping.insert("T".to_string(), 1);
       score_mapping.insert("a".to_string(), 2);
       score_mapping.insert("A".to_string(), 1);
       score_mapping.insert("o".to_string(), 2);
       score_mapping.insert("O".to_string(), 1);
       score_mapping.insert("i".to_string(), 2);
       score_mapping.insert("I".to_string(), 1);
       score_mapping.insert("n".to_string(), 2);
       score_mapping.insert("N".to_string(), 1);
       score_mapping.insert("s".to_string(), 2);
       score_mapping.insert("S".to_string(), 1);
       score_mapping.insert("h".to_string(), 2);
       score_mapping.insert("H".to_string(), 1);
       score_mapping.insert("r".to_string(), 2);
       score_mapping.insert("R".to_string(), 1);
       score_mapping.insert("d".to_string(), 2);
       score_mapping.insert("D".to_string(), 1);
       score_mapping.insert("l".to_string(), 2);
       score_mapping.insert("L".to_string(), 1);
       score_mapping.insert("u".to_string(), 2);
       score_mapping.insert("U".to_string(), 1);
       score_mapping.insert(" ".to_string(), 2); //don't forget space!

    for c in input.chars() {
        score += score_mapping.get(&c.to_string()).unwrap_or(&0);
    }

    score
}