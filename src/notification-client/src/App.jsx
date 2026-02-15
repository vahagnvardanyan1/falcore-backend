import React, { useEffect, useState, useRef } from 'react'
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr'

window.__BACKEND_URL__ = "https://localhost:7067";

export default function App() {
  const [connection, setConnection] = useState(null)
  const [connected, setConnected] = useState(false)
  const [tenantId, setTenantId] = useState('')
  const [notifications, setNotifications] = useState([])
  const connectionRef = useRef(null)

  useEffect(() => {
    const url = (window.__BACKEND_URL__ || window.location.origin) + '/hubs/notifications'

    const conn = new HubConnectionBuilder()
      .withUrl(url)
      .configureLogging(LogLevel.Information)
      .withAutomaticReconnect()
      .build()

    conn.on('ReceiveNotification', (payload) => {
      // payload expected to be { tenantId, vehicleId, title, message, timestampUtc }
      setNotifications(prev => [{ ...payload }, ...prev])
    })

    conn.start()
      .then(() => {
        setConnected(true)
        connectionRef.current = conn
        setConnection(conn)
      })
      .catch(e => console.error('Connection failed: ', e))

    return () => {
      conn.stop().catch(() => {})
    }
  }, [])

  const register = async () => {
    if (!connection) return
    if (!tenantId) return
    try {
      await connection.invoke('RegisterTenant', Number(tenantId))
      console.info('Registered for tenant', tenantId)
    } catch (e) {
      console.error(e)
    }
  }

  const unregister = async () => {
    if (!connection) return
    if (!tenantId) return
    try {
      await connection.invoke('UnregisterTenant', Number(tenantId))
      console.info('Unregistered for tenant', tenantId)
    } catch (e) {
      console.error(e)
    }
  }

  return (
    <div className="app">
      <header>
        <h1>VTS Notifications</h1>
        <div className="status">Connection: {connected ? 'Connected' : 'Disconnected'}</div>
      </header>

      <section className="controls">
        <label>
          Tenant ID:
          <input value={tenantId} onChange={e => setTenantId(e.target.value)} placeholder="enter tenant id" />
        </label>
        <div className="buttons">
          <button onClick={register} disabled={!connected || !tenantId}>Register</button>
          <button onClick={unregister} disabled={!connected || !tenantId}>Unregister</button>
        </div>
      </section>

      <section className="notifications">
        <h2>Notifications</h2>
        {notifications.length === 0 && <div className="empty">No notifications yet</div>}
        <ul>
          {notifications.map((n, idx) => (
            <li key={idx} className="notification">
              <div className="meta">
                <strong>{n.title}</strong>
                <span className="time">{new Date(n.timestampUtc).toLocaleString()}</span>
              </div>
              <div className="message">{n.message}</div>
              <div className="vehicle">Vehicle ID: {n.vehicleId}</div>
            </li>
          ))}
        </ul>
      </section>
    </div>
  )
}
