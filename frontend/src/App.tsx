import React, { useState, useEffect, useCallback } from 'react';
import './App.css';

interface Book {
  id: number;
  title: string;
  author: string;
  price: number;
}

function App() {
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true);
  const [cart, setCart] = useState<Book[]>([]);
  const [error, setError] = useState('');

  const API_BASE = process.env.REACT_APP_API_URL || 'http://localhost:5000';


  const fetchBooks = useCallback(async () => {
    try {
      const response = await fetch(`${API_BASE}/api/books`);
      if (response.ok) {
        const data: Book[] = await response.json();
        setBooks(data);
      } else {
        setError('Failed to fetch books');
      }
    } catch (err) {
      setError('Error connecting to server');
    } finally {
      setLoading(false);
    }

  }, []);

    useEffect(() => {
    fetchBooks();
  }, [fetchBooks]);


  const addToCart = (book: Book) => {
    setCart([...cart, book]);
    alert(`Added "${book.title}" to cart!`);
  };

  const removeFromCart = (bookId: number) => {
    setCart(cart.filter(book => book.id !== bookId));
  };

  const getTotalPrice = () => {
    return cart.reduce((total, book) => total + book.price, 0).toFixed(2);
  };

  if (loading) return <div className="loading">Loading books...</div>;
  if (error) return <div className="error">Error: {error}</div>;

  return (
    <div className="App">
      <header className="App-header">
        <h1>📚 BookStore</h1>
        <div className="cart-info">
          Cart: {cart.length} items (${getTotalPrice()})
        </div>
      </header>

      <main className="main-content">
        <section className="books-section">
          <h2>Available Books</h2>
          <div className="books-grid">
            {books.map(book => (
              <div key={book.id} className="book-card">
                <h3>{book.title}</h3>
                <p className="author">by {book.author}</p>
                <p className="price">${book.price}</p>
                <button 
                  onClick={() => addToCart(book)}
                  className="add-to-cart-btn"
                >
                  Add to Cart
                </button>
              </div>
            ))}
          </div>
        </section>

        <section className="cart-section">
          <h2>Shopping Cart</h2>
          {cart.length === 0 ? (
            <p>Your cart is empty</p>
          ) : (
            <div className="cart-items">
              {cart.map((book, index) => (
                <div key={index} className="cart-item">
                  <span>{book.title} - ${book.price}</span>
                  <button 
                    onClick={() => removeFromCart(book.id)}
                    className="remove-btn"
                  >
                    Remove
                  </button>
                </div>
              ))}
              <div className="cart-total">
                <strong>Total: ${getTotalPrice()}</strong>
              </div>
              <button className="checkout-btn">
                Checkout
              </button>
            </div>
          )}
        </section>
      </main>

      <footer>
        <p>Environment: {process.env.REACT_APP_ENVIRONMENT || 'development'}</p>
      </footer>
    </div>
  );
}

export default App;