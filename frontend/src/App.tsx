import { useState } from 'react';
import { 
  Box, 
  Button, 
  Container, 
  TextField, 
  Typography,
  CssBaseline,
  ThemeProvider,
  createTheme,
  CircularProgress,
  Select,
  MenuItem,
  FormControl,
  InputLabel
} from '@mui/material';
import axios, { AxiosError } from 'axios';

// Тёмная тема
const darkTheme = createTheme({
  palette: {
    mode: 'dark',
    background: {
      default: '#121212',
      paper: '#1E1E1E'
    },
    primary: {
      main: '#90caf9',
    },
    secondary: {
      main: '#f48fb1',
    },
  },
});

interface User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  role: string;
}

interface Order {
  id: number;
  address: string;
  status: string;
  customerId: number;
  courierId?: number;
}

function App() {
  const [token, setToken] = useState<string>('');
  const [email, setEmail] = useState<string>('admin@example.com');
  const [password, setPassword] = useState<string>('Admin123!');
  const [response, setResponse] = useState<any>(null);
  const [loading, setLoading] = useState<boolean>(false);
  
  // Данные для форм
  const [orderAddress, setOrderAddress] = useState<string>('ул. Пушкина, 10');
  const [ratingData, setRatingData] = useState({
    orderId: 1,
    score: 5,
    comment: 'Отличная доставка!'
  });
  const [newUser, setNewUser] = useState({
    firstName: 'Новый',
    lastName: 'Пользователь',
    email: 'new@example.com',
    password: 'Password123!',
    role: 'Customer'
  });

  const login = async () => {
    setLoading(true);
    try {
      const res = await axios.post('/api/auth/login', { email, password });
      setToken(res.data.token);
      setResponse(res.data);
    } catch (error) {
      setResponse({
        error: 'Ошибка авторизации',
        details: (error as AxiosError).response?.data
      });
    } finally {
      setLoading(false);
    }
  };

  const apiRequest = async (method: string, url: string, data?: any) => {
    setLoading(true);
    try {
      const res = await axios({
        method,
        url: `/api${url}`,
        data,
        headers: { Authorization: `Bearer ${token}` }
      });
      setResponse(res.data);
    } catch (error) {
      setResponse({
        error: 'Ошибка запроса',
        details: (error as AxiosError).response?.data
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <ThemeProvider theme={darkTheme}>
      <CssBaseline />
      <Container maxWidth="lg">
        <Box sx={{ my: 4 }}>
          <Typography variant="h4" color="primary" gutterBottom>
            Панель тестирования DeliveryService API
          </Typography>

          {/* Секция авторизации */}
          <Box sx={{ p: 3, mb: 4, bgcolor: 'background.paper', borderRadius: 2 }}>
            <Typography variant="h6" gutterBottom>Авторизация</Typography>
            <Box sx={{ display: 'flex', gap: 2 }}>
              <TextField
                label="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                fullWidth
              />
              <TextField
                label="Password"
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                fullWidth
              />
              <Button 
                variant="contained" 
                onClick={login}
                disabled={loading}
                sx={{ minWidth: 120 }}
              >
                {loading ? <CircularProgress size={24} /> : 'Войти'}
              </Button>
            </Box>
            {token && (
              <Typography sx={{ mt: 2, wordBreak: 'break-all' }}>
                <strong>Токен:</strong> {token.slice(0, 30)}...
              </Typography>
            )}
          </Box>

          {/* Секция работы с пользователями */}
          <Box sx={{ p: 3, mb: 4, bgcolor: 'background.paper', borderRadius: 2 }}>
            <Typography variant="h6" gutterBottom>Пользователи</Typography>
            <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap', mb: 2 }}>
              <Button 
                variant="outlined" 
                onClick={() => apiRequest('get', '/users')}
                disabled={!token}
              >
                Получить всех
              </Button>
              <Button 
                variant="outlined" 
                onClick={() => apiRequest('get', '/users/1')}
                disabled={!token}
              >
                Получить по ID
              </Button>
            </Box>
            
            <Typography variant="subtitle1" sx={{ mt: 2 }}>Создать пользователя:</Typography>
            <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap' }}>
              <TextField
                label="Имя"
                value={newUser.firstName}
                onChange={(e) => setNewUser({...newUser, firstName: e.target.value})}
              />
              <TextField
                label="Фамилия"
                value={newUser.lastName}
                onChange={(e) => setNewUser({...newUser, lastName: e.target.value})}
              />
              <TextField
                label="Email"
                value={newUser.email}
                onChange={(e) => setNewUser({...newUser, email: e.target.value})}
              />
              <TextField
                label="Пароль"
                type="password"
                value={newUser.password}
                onChange={(e) => setNewUser({...newUser, password: e.target.value})}
              />
              <FormControl sx={{ minWidth: 120 }}>
                <InputLabel>Роль</InputLabel>
                <Select
                  value={newUser.role}
                  label="Роль"
                  onChange={(e) => setNewUser({...newUser, role: e.target.value})}
                >
                  <MenuItem value="Admin">Admin</MenuItem>
                  <MenuItem value="Customer">Customer</MenuItem>
                  <MenuItem value="Courier">Courier</MenuItem>
                </Select>
              </FormControl>
              <Button 
                variant="contained" 
                onClick={() => apiRequest('post', '/users', newUser)}
                disabled={!token}
              >
                Создать
              </Button>
            </Box>
          </Box>

          {/* Секция работы с заказами */}
          <Box sx={{ p: 3, mb: 4, bgcolor: 'background.paper', borderRadius: 2 }}>
            <Typography variant="h6" gutterBottom>Заказы</Typography>
            <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap', mb: 2 }}>
              <Button 
                variant="outlined" 
                onClick={() => apiRequest('get', '/orders')}
                disabled={!token}
              >
                Получить все
              </Button>
              <Button 
                variant="outlined" 
                onClick={() => apiRequest('get', '/orders/1')}
                disabled={!token}
              >
                Получить по ID
              </Button>
            </Box>
            
            <Typography variant="subtitle1" sx={{ mt: 2 }}>Создать заказ:</Typography>
            <Box sx={{ display: 'flex', gap: 2, alignItems: 'center' }}>
              <TextField
                label="Адрес"
                value={orderAddress}
                onChange={(e) => setOrderAddress(e.target.value)}
                sx={{ flexGrow: 1 }}
              />
              <Button 
                variant="contained" 
                onClick={() => apiRequest('post', '/orders', { address: orderAddress })}
                disabled={!token}
              >
                Создать
              </Button>
            </Box>
            
            <Typography variant="subtitle1" sx={{ mt: 3 }}>Действия с заказом:</Typography>
            <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap' }}>
              <Button 
                variant="outlined" 
                onClick={() => apiRequest('patch', '/orders/1/accept')}
                disabled={!token}
              >
                Принять заказ (Courier)
              </Button>
              <Button 
                variant="outlined" 
                onClick={() => apiRequest('patch', '/orders/1/complete')}
                disabled={!token}
              >
                Завершить заказ (Courier)
              </Button>
            </Box>
          </Box>

          {/* Секция работы с оценками */}
          <Box sx={{ p: 3, bgcolor: 'background.paper', borderRadius: 2 }}>
            <Typography variant="h6" gutterBottom>Оценки</Typography>
            <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap' }}>
              <TextField
                label="ID заказа"
                type="number"
                value={ratingData.orderId}
                onChange={(e) => setRatingData({...ratingData, orderId: Number(e.target.value)})}
              />
              <TextField
                label="Оценка (1-5)"
                type="number"
                inputProps={{ min: 1, max: 5 }}
                value={ratingData.score}
                onChange={(e) => setRatingData({...ratingData, score: Number(e.target.value)})}
              />
              <TextField
                label="Комментарий"
                value={ratingData.comment}
                onChange={(e) => setRatingData({...ratingData, comment: e.target.value})}
                sx={{ flexGrow: 1 }}
              />
              <Button 
                variant="contained" 
                onClick={() => apiRequest('post', '/ratings', ratingData)}
                disabled={!token}
              >
                Добавить оценку
              </Button>
            </Box>
          </Box>

          {/* Секция с ответом сервера */}
          <Box sx={{ mt: 4, p: 3, bgcolor: 'background.paper', borderRadius: 2 }}>
            <Typography variant="h6" gutterBottom>Ответ сервера</Typography>
            <Box component="pre" sx={{ 
              p: 2, 
              bgcolor: '#252525', 
              borderRadius: 1, 
              overflowX: 'auto',
              maxHeight: '300px',
              overflowY: 'auto'
            }}>
              {loading ? 'Загрузка...' : JSON.stringify(response, null, 2)}
            </Box>
          </Box>
        </Box>
      </Container>
    </ThemeProvider>
  );
}

export default App;