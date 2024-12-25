import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import LoginPage from './pages/LoginPage';
import HomePage from './pages/HomePage';
import RegisterPage from './pages/RegisterPage';

function App() {
  return (
    <AuthProvider>
      <Router>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
        </Routes>
      </Router>
    </AuthProvider>
  )
}

export default App
