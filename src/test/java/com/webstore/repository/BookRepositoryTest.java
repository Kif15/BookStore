package com.webstore.repository;

import com.webstore.model.Book;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;

import java.util.List;
import java.util.Optional;

import static org.junit.jupiter.api.Assertions.*;

@DataJpaTest
class BookRepositoryTest {
    
    @Autowired
    private BookRepository bookRepository;
    
    @Test
    void testSaveBook() {
        Book book = new Book("Test Book", "Test Author", "Description", 15.99, "url");
        Book savedBook = bookRepository.save(book);
        
        assertNotNull(savedBook.getId());
        assertEquals("Test Book", savedBook.getTitle());
    }
    
    @Test
    void testFindById() {
        Book book = new Book("Find Me", "Author", "Description", 20.0, "url");
        Book savedBook = bookRepository.save(book);
        
        Optional<Book> foundBook = bookRepository.findById(savedBook.getId());
        
        assertTrue(foundBook.isPresent());
        assertEquals("Find Me", foundBook.get().getTitle());
    }
    
    @Test
    void testFindAll() {
        bookRepository.save(new Book("Book 1", "Author 1", "Desc 1", 10.0, "url1"));
        bookRepository.save(new Book("Book 2", "Author 2", "Desc 2", 20.0, "url2"));
        
        List<Book> books = bookRepository.findAll();
        
        assertTrue(books.size() >= 2);
    }
    
    @Test
    void testDeleteBook() {
        Book book = new Book("Delete Me", "Author", "Description", 15.0, "url");
        Book savedBook = bookRepository.save(book);
        Long id = savedBook.getId();
        
        bookRepository.deleteById(id);
        
        Optional<Book> deletedBook = bookRepository.findById(id);
        assertFalse(deletedBook.isPresent());
    }
}