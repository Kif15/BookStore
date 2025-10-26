package com.webstore.config;

import com.webstore.model.Book;
import com.webstore.repository.BookRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.CommandLineRunner;
import org.springframework.stereotype.Component;

@Component
public class DataInitializer implements CommandLineRunner {
    
    @Autowired
    private BookRepository bookRepository;
    
    @Override
    public void run(String... args) throws Exception {
        // Check if books already exist
        if (bookRepository.count() == 0) {
            // Add sample books
            bookRepository.save(new Book(
                "The Great Gatsby",
                "F. Scott Fitzgerald",
                "The Great Gatsby is a 1925 novel by American writer F. Scott Fitzgerald. Set in the Jazz Age on Long Island, near New York City, the novel depicts first-person narrator Nick Carraway's interactions with mysterious millionaire Jay Gatsby and Gatsby's obsession to reunite with his former lover, Daisy Buchanan.",
                12.99,
                "https://images.unsplash.com/photo-1543002588-bfa74002ed7e?w=400"
            ));
            
            bookRepository.save(new Book(
                "To Kill a Mockingbird",
                "Harper Lee",
                "To Kill a Mockingbird is a novel by the American author Harper Lee. It was published in 1960 and was instantly successful. In the United States, it is widely read in high schools and middle schools. The story takes place in the fictional town of Maycomb, Alabama, during the Great Depression.",
                14.99,
                "https://images.unsplash.com/photo-1512820790803-83ca734da794?w=400"
            ));
            
            bookRepository.save(new Book(
                "1984",
                "George Orwell",
                "1984 is a dystopian social science fiction novel and cautionary tale by English writer George Orwell. It was published on 8 June 1949 by Secker & Warburg as Orwell's ninth and final book completed in his lifetime. The story takes place in an imagined future in the year 1984.",
                13.99,
                "https://images.unsplash.com/photo-1495446815901-a7297e633e8d?w=400"
            ));
            
            bookRepository.save(new Book(
                "Pride and Prejudice",
                "Jane Austen",
                "Pride and Prejudice is an 1813 novel of manners by Jane Austen. The novel follows the character development of Elizabeth Bennet, the protagonist of the book, who learns about the repercussions of hasty judgments and comes to appreciate the difference between superficial goodness and actual goodness.",
                11.99,
                "https://images.unsplash.com/photo-1544947950-fa07a98d237f?w=400"
            ));
            
            bookRepository.save(new Book(
                "The Catcher in the Rye",
                "J.D. Salinger",
                "The Catcher in the Rye is a novel by J. D. Salinger. A controversial novel originally published for adults, it has since become popular with adolescent readers for its themes of teenage angst and alienation. The novel's protagonist Holden Caulfield has become an icon for teenage rebellion.",
                15.99,
                "https://images.unsplash.com/photo-1524995997946-a1c2e315a42f?w=400"
            ));
            
            bookRepository.save(new Book(
                "Harry Potter and the Sorcerer's Stone",
                "J.K. Rowling",
                "Harry Potter and the Philosopher's Stone is a fantasy novel written by British author J. K. Rowling. The first novel in the Harry Potter series, it follows Harry Potter, a young wizard who discovers his magical heritage on his eleventh birthday.",
                19.99,
                "https://images.unsplash.com/photo-1621351183012-e2f9972dd9bf?w=400"
            ));
            
            bookRepository.save(new Book(
                "The Hobbit",
                "J.R.R. Tolkien",
                "The Hobbit, or There and Back Again is a children's fantasy novel by English author J. R. R. Tolkien. It was published in 1937 to wide critical acclaim. The story follows the quest of home-loving Bilbo Baggins, the titular hobbit, to win a share of the treasure guarded by a dragon named Smaug.",
                16.99,
                "https://images.unsplash.com/photo-1589998059171-988d887df646?w=400"
            ));
            
            bookRepository.save(new Book(
                "The Lord of the Rings",
                "J.R.R. Tolkien",
                "The Lord of the Rings is an epic high-fantasy novel by English author and scholar J. R. R. Tolkien. Set in Middle-earth, the story began as a sequel to Tolkien's 1937 children's book The Hobbit, but eventually developed into a much larger work.",
                24.99,
                "https://images.unsplash.com/photo-1621944190310-e3cca1564bd7?w=400"
            ));
            
            System.out.println("Sample books have been added to the database!");
        }
    }
}