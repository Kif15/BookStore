package com.webstore.repository;

import com.webstore.model.Book;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface BookRepository extends JpaRepository<Book, Long> {
    // JpaRepository provides all basic CRUD operations
    // Additional custom query methods can be added here if needed
}