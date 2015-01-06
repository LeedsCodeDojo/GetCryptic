package io.github.leedscodedojo;

import org.junit.Test;

import static org.hamcrest.CoreMatchers.is;
import static org.junit.Assert.assertThat;

public class MyClassTests {
    @Test
    public void helloWorld() {
        MyClass myClass = new MyClass();

        assertThat(myClass.helloWorld(), is("Hello World!"));
    }
}
