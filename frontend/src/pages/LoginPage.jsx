import React, { useState } from 'react';
import api from '../api/axios';

function parseJwt(token) {
  try {
    return JSON.parse(atob(token.split('.')[1]));
  } catch (e) {
    return null;
  }
}

export default function LoginPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = async () => {
    try {
      const response = await api.post('/auth/login', { email, password });
      console.log('Full response:', response);
      console.log('Response data:', response.data);

      const token = response.data.token;  
      localStorage.setItem('token', token);

      const decoded = parseJwt(token);
      console.log('Decoded token:', decoded);

      const role = decoded?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
      console.log('Полученная роль из токена:', role);

      localStorage.setItem('role', role);

      switch (role) {
        case 'Admin':
          window.location.href = '/admin';
          break;
        case 'Courier':
          window.location.href = '/courier';
          break;
        case 'Customer':
          window.location.href = '/customer';
          break;
        default:
          alert('Неизвестная роль');
          break;
      }
    } catch (err) {
      console.error('Login error:', err);
      alert('Ошибка входа');
    }
  };

  return (
    <div>
      <h2>Вход</h2>
      <input
        placeholder="Email"
        value={email}
        onChange={e => setEmail(e.target.value)}
      />
      <input
        placeholder="Пароль"
        type="password"
        value={password}
        onChange={e => setPassword(e.target.value)}
      />
      <button onClick={handleLogin}>Войти</button>
    </div>
  );
}
