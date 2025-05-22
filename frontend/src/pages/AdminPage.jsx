import React, { useEffect, useState } from 'react';
import { getOrders } from '../api/orders';

export default function AdminPage() {
  const [orders, setOrders] = useState([]);

  useEffect(() => {
    getOrders().then(setOrders);
  }, []);

  return (
    <div>
      <h2>Заказы</h2>
      <ul>
        {orders.map(order => (
          <li key={order.id}>
            {order.customerName} — {order.status}
          </li>
        ))}
      </ul>
    </div>
  );
}
