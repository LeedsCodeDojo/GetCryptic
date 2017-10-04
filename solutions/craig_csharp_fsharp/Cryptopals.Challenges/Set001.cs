using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;
using Shouldly;
using Buffer = Cryptopals.Core.Buffer;

namespace Cryptopals.Challenges
{
    class Set001 {
        readonly string _dataPath = TestContext.CurrentContext.TestDirectory;

        [Test]
        public void Challenge001() {
            var fromHex = Buffer.FromHex("49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d");

            var toBase64 = fromHex.ToBase64();

            toBase64.ShouldBe("SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t");
        }

        [Test]
        public void Challenge002() {
            var buffer1 = Buffer.FromHex("1c0111001f010100061a024b53535009181c");
            var buffer2 = Buffer.FromHex("686974207468652062756c6c277320657965");

            var xorBuffer = buffer1 ^ buffer2;
            var hex = xorBuffer.ToHex();

            hex.ShouldBe("746865206b696420646f6e277420706c6179");
        }

        [Test]
        public void Challenge003() {
            var buffer = Buffer.FromHex("1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736");
            var scored = buffer.EnglishScore();

            scored.Text.ShouldBe("Cooking MC's like a pound of bacon");
        }

        [Test]
        public void Challenge004() {
            var buffers = File.ReadAllLines(Path.Combine(_dataPath, "Set001Challenge004.txt")).Select(Buffer.FromHex);

            (int Score, byte Byte, string Text) bestScore = (0, 0, "");
            
            foreach (var buffer in buffers) {
                var scored = buffer.EnglishScore();
                if (scored.Score <= bestScore.Score) continue;
                bestScore = scored;
            }

            bestScore.Text.Trim().ShouldBe("Now that the party is jumping");
        }

        [Test]
        public void Challenge005() {
            const string toEncrypt = "Burning 'em, if you ain't quick and nimble\n" + "I go crazy when I hear a cymbal";

            var buffer = Buffer.FromText(toEncrypt) ^ "ICE";
            var asHex = buffer.ToHex();

            asHex.ShouldBe("0b3637272a2b2e63622c2e69692a23693a2a3c6324202d623d63343c2a26226324272765272" + "a282b2f20430a652e2c652a3124333a653e2b2027630c692b20283165286326302e27282f");
        }

        [Test]
        public void Challenge006_HammingDistance()
        {
            Buffer.FromText("this is a test").HammingDistance(Buffer.FromText("wokka wokka!!!")).ShouldBe(37);
        }

        [Test]
        public void Challenge006() {
            var lines = File.ReadAllLines(Path.Combine(_dataPath, "Set001Challenge006.txt"));
            var content = string.Join("", lines);
            var buffer = Buffer.FromBase64(content);
            
            var likelihoods = 
                Enumerable.Range(2, 40).Select(blocksize => new {
                    BlockSize = blocksize,
                    Likelihood = 
                        Enumerable.Range(0, 10).Select(block => {
                            var buffer1 = buffer.GetBlock(blocksize, block * 2);
                            var buffer2 = buffer.GetBlock(blocksize, block * 2 + 1);
                            return buffer1.NormalisedHammingDistance(buffer2);
                        }).Average()
                })
                .OrderBy(m => m.Likelihood);


            var blockSize = likelihoods.First().BlockSize;

            var transposed = buffer.Select((value, index) => new { Value = value, Index = index })
                .GroupBy(x => x.Index % blockSize)
                .Select(grp => grp.Select(x => x.Value).ToArray())
                .Select(m => new Buffer(m))
                .Select(m => m.EnglishScore())
                .Select(score => Convert.ToChar(score.Byte))
                .ToArray();

            var key = new string(transposed);
            var message = (buffer ^ key).ToText();
            
            blockSize.ShouldBe(29);
            key.ShouldBe("Terminator X: Bring the noise");
            message.Split('\n')[0].ShouldBe("I'm back and I'm ringin' the bell ");
        }

        [Test]
        public void Challenge007() {
            var lines = File.ReadAllLines(Path.Combine(_dataPath, "Set001Challenge007.txt"));
            var content = string.Join("", lines);
            var buffer = Buffer.FromBase64(content);

            var outputBuffer = new byte[buffer.Length];

            var aes = Aes.Create();
            aes.Mode = CipherMode.ECB;
            aes.BlockSize = 128;
            aes.Key = Encoding.ASCII.GetBytes("YELLOW SUBMARINE");
            aes.CreateDecryptor().TransformBlock(buffer, 0, buffer.Length, outputBuffer, 0);

            var text = new Buffer(outputBuffer).ToText();

            text.Split('\n')[0].ShouldBe("I'm back and I'm ringin' the bell ");
        }
    }
}
