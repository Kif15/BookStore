package com.webstore.model;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import org.junit.jupiter.api.Test;

class BookTest {
    
    @Test
    void testBookCreation() {
        Book book = new Book("Test Title", "Test Author", "Test Description", 19.99, "http://test.com/image.jpg");
        
        assertEquals("Test Title", book.getTitle());
        assertEquals("Test Author", book.getAuthor());
        assertEquals("Test Description", book.getDescription());
        assertEquals(19.99, book.getPrice());
        assertEquals("http://test.com/image.jpg", book.getImageUrl());
    }
    
    @Test
    void testBookSetters() {
        Book book = new Book();
        book.setTitle("New Title");
        book.setAuthor("New Author");
        book.setDescription("New Description");
        book.setPrice(29.99);
        book.setImageUrl("http://new.com/image.jpg");
        
        assertEquals("New Title", book.getTitle());
        assertEquals("New Author", book.getAuthor());
        assertEquals("New Description", book.getDescription());
        assertEquals(29.99, book.getPrice());
        assertEquals("http://new.com/image.jpg", book.getImageUrl());
    }
    
    @Test
    void testPriceNotNull() {
        Book book = new Book("Title", "Author", "Description", 10.0, "url");
        assertNotNull(book.getPrice());
    }
}