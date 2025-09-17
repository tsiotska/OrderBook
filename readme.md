# OrderBook Project

This repository contains:

- **Backend**: ASP.NET Core 9 Web API (`OrderBook.Api`)
- **Frontend**: Vue 3 + Vite + Tailwind (`OrderBook.Client`)

The project fetches BTC/EUR order book data, exposes it over a REST API & SignalR, and displays it in real time on a Vue dashboard with a TradingView chart and order book visualization.

---

## 🚀 Prerequisites

- [Node.js](https://nodejs.org/) (>=18.x recommended)
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- npm (comes with Node)

---

## Frontend (Vue)
### 📦 Install dependencies

```bash
cd OrderBook.Client
npm install
```

### ▶️ Running
```bash
npm run dev
```

### 🧪 Running Tests
```bash
npm run test
```

## Backend (.NET 9)
```bash
cd OrderBookApi/OrderBook.Api
dotnet restore
```

### ▶️ Running
```bash
dotnet run
```

### 🧪 Running Tests
```bash
cd OrderBookApi/OrderBook.Tests
dotnet test
```

