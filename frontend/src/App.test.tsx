import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import App from './App';

type Book = {
  id: number;
  title: string;
  author: string;
  price: number;
};

// Mock fetch properly for TypeScript
const mockFetch = jest.fn() as jest.MockedFunction<typeof fetch>;

// Replace global fetch with our mock
Object.defineProperty(window, 'fetch', {
  value: mockFetch,
  writable: true,
});

describe('BookStore App', () => {
  beforeEach(() => {
    mockFetch.mockClear();
    // Clear any previous window.alert mocks
    jest.clearAllMocks();
  });

  test('renders bookstore header', async () => {
    mockFetch.mockResolvedValueOnce({
      ok: true,
      json: async (): Promise<Book[]> => [
        { id: 1, title: 'Test Book', author: 'Test Author', price: 10.99 }
      ]
    } as Response);

    render(<App />);
    const headerElement = screen.getByText(/📚 BookStore/i);
    expect(headerElement).toBeInTheDocument();
  });

  test('displays loading state initially', () => {
    mockFetch.mockImplementationOnce(() => new Promise(() => {}));

    render(<App />);
    const loadingElement = screen.getByText(/Loading books.../i);
    expect(loadingElement).toBeInTheDocument();
  });

  test('displays books after loading', async () => {
    const mockBooks: Book[] = [
      { id: 1, title: 'The Great Gatsby', author: 'F. Scott Fitzgerald', price: 12.99 },
      { id: 2, title: '1984', author: 'George Orwell', price: 13.99 }
    ];

    mockFetch.mockResolvedValueOnce({
      ok: true,
      json: async (): Promise<Book[]> => mockBooks
    } as Response);

    render(<App />);

    await waitFor(() => {
      expect(screen.getByText('The Great Gatsby')).toBeInTheDocument();
      expect(screen.getByText('by F. Scott Fitzgerald')).toBeInTheDocument();
      expect(screen.getByText('$12.99')).toBeInTheDocument();
    });
  });

  test('displays error message when API fails', async () => {
    mockFetch.mockResolvedValueOnce({ 
      ok: false 
    } as Response);

    render(<App />);

    await waitFor(() => {
      expect(screen.getByText(/Error: Failed to fetch books/i)).toBeInTheDocument();
    });
  });

  test('adds book to cart when add to cart button is clicked', async () => {
    const mockBooks: Book[] = [
      { id: 1, title: 'Test Book', author: 'Test Author', price: 10.99 }
    ];

    mockFetch.mockResolvedValueOnce({
      ok: true,
      json: async (): Promise<Book[]> => mockBooks
    } as Response);

    // Mock window.alert
    const mockAlert = jest.fn();
    Object.defineProperty(window, 'alert', {
      value: mockAlert,
      writable: true,
    });

    render(<App />);

    await waitFor(() => {
      expect(screen.getByText('Test Book')).toBeInTheDocument();
    });

    const addToCartButton = screen.getByText('Add to Cart');
    fireEvent.click(addToCartButton);

    expect(screen.getByText(/Cart: 1 items/i)).toBeInTheDocument();
    expect(mockAlert).toHaveBeenCalledWith('Added "Test Book" to cart!');
  });

  test('removes book from cart when remove button is clicked', async () => {
    const mockBooks: Book[] = [
      { id: 1, title: 'Test Book', author: 'Test Author', price: 10.99 }
    ];

    mockFetch.mockResolvedValueOnce({
      ok: true,
      json: async (): Promise<Book[]> => mockBooks
    } as Response);

    // Mock window.alert
    const mockAlert = jest.fn();
    Object.defineProperty(window, 'alert', {
      value: mockAlert,
      writable: true,
    });

    render(<App />);

    await waitFor(() => {
      expect(screen.getByText('Test Book')).toBeInTheDocument();
    });

    const addToCartButton = screen.getByText('Add to Cart');
    fireEvent.click(addToCartButton);

    expect(screen.getByText(/Cart: 1 items/i)).toBeInTheDocument();

    const removeButton = screen.getByText('Remove');
    fireEvent.click(removeButton);

    expect(screen.getByText(/Cart: 0 items/i)).toBeInTheDocument();
  });

  test('calculates total price correctly', async () => {
    const mockBooks: Book[] = [
      { id: 1, title: 'Book 1', author: 'Author 1', price: 10.99 },
      { id: 2, title: 'Book 2', author: 'Author 2', price: 15.99 }
    ];

    mockFetch.mockResolvedValueOnce({
      ok: true,
      json: async (): Promise<Book[]> => mockBooks
    } as Response);

    // Mock window.alert
    const mockAlert = jest.fn();
    Object.defineProperty(window, 'alert', {
      value: mockAlert,
      writable: true,
    });

    render(<App />);

    await waitFor(() => {
      expect(screen.getByText('Book 1')).toBeInTheDocument();
    });

    const addToCartButtons = screen.getAllByText('Add to Cart');
    fireEvent.click(addToCartButtons[0]);
    fireEvent.click(addToCartButtons[1]);

    expect(screen.getByText(/Cart: 2 items \(\$26\.98\)/i)).toBeInTheDocument();
    expect(screen.getByText(/Total: \$26\.98/i)).toBeInTheDocument();
  });

  test('displays environment information', async () => {
    // Set environment variable for this test
    const originalEnv = process.env.REACT_APP_ENVIRONMENT;
    process.env.REACT_APP_ENVIRONMENT = 'test';

    mockFetch.mockResolvedValueOnce({
      ok: true,
      json: async (): Promise<Book[]> => []
    } as Response);

    render(<App />);

    await waitFor(() => {
      expect(screen.getByText(/Environment: test/i)).toBeInTheDocument();
    });

    // Restore original environment
    process.env.REACT_APP_ENVIRONMENT = originalEnv;
  });
});