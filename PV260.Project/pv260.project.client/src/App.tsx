import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Homepage from '@/pages/homepage.tsx';
import { LoginPage } from '@/pages/login-page.tsx';
import { SignupPage } from '@/pages/signup-page.tsx';


const App = () => {

  return (
    <Router>
      <Routes>
        <Route path="/" element={<Homepage/>} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/signup" element={<SignupPage />} />
      </Routes>
    </Router>

  );
};

export default App;
