import React, { useEffect, useState } from 'react';
import api from '../api';
import { Typography, Container, Button, List, ListItem } from '@mui/material';

type Order = {
  id: number;
  description: string;
  status: string;
};

const Dashboard: React.FC = () => {
  const [role, setRole] = useState<string | null>(null);
  const [orders, setOrders] = useState<Order[]>([]);

  useEffect(() => {
    const fetchRole = async () => {
      try {
        const res = await api.get('/users/me'); // Эндпоинт для получения данных пользователя
        setRole(res.data.role);
      } catch (e) {
        console.error(e);
      }
    };
    fetchRole();
  }, []);

  useEffect(() => {
    if (!role) return;

    const fetchOrders = async () => {
      try {
        if (role === 'Customer') {
          const res = await api.get('/orders/my');
          setOrders(res.data);
        } else if (role === 'Courier') {
          const res = await api.get('/orders/available');
          setOrders(res.data);
        } else if (role === 'Admin') {
          const res = await api.get('/orders/all');
          setOrders(res.data);
        }
      } catch (e) {
        console.error(e);
      }
    };

    fetchOrders();
  }, [role]);

  // if (!role) return <Typography>Загрузка...</Typography>;

  return (
    <Container>
      <Typography variant="h4" mt={2}>Панель {role}</Typography>
      <List>
        {orders.map(order => (
          <ListItem key={order.id}>
            {order.description} - Статус: {order.status}
          </ListItem>
        ))}
      </List>
      {/* Можно добавить кнопки и функционал создания/принятия заказов */}
    </Container>
  );
};

export default Dashboard;
