package com.webstore.controller;

import com.webstore.model.Book;
import com.webstore.repository.BookRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;

import java.util.List;

@Controller
public class BookController {
    
    @Autowired
    private BookRepository bookRepository;
    
    @GetMapping("/")
    public String home() {
        return "redirect:/books";
    }
    
    @GetMapping("/books")
    public String listBooks(Model model) {
        List<Book> books = bookRepository.findAll();
        model.addAttribute("books", books);
        return "book-list";
    }
    
    @GetMapping("/books/{id}")
    public String bookDetails(@PathVariable Long id, Model model) {
        Book book = bookRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Book not found with id: " + id));
        model.addAttribute("book", book);
        return "book-details";
    }
}