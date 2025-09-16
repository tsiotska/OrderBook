<template>
  <div class="orderbook">
    <h2>BTC/EUR Order Book</h2>

    <BarChart v-if="chartData" :chart-data="chartData" :options="chartOptions" class="chart"/>

    <div class="flex">
      <div class="list">
        <h3>Asks (Sellers)</h3>
        <ul>
          <li v-for="(ask, i) in snapshot.asks.slice(0, 5)" :key="'ask' + i">
            {{ ask.amount }} BTC @ €{{ ask.price }}
          </li>
        </ul>
      </div>

      <div class="list">
        <h3>Bids (Buyers)</h3>
        <ul>
          <li v-for="(bid, i) in snapshot.bids.slice(0, 5)" :key="'bid' + i">
            {{ bid.amount }} BTC @ €{{ bid.price }}
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>

<script setup>
import {ref, onMounted} from "vue"
import * as signalR from "@microsoft/signalr"
import Chart from "chart.js/auto"
import {BarChart} from "vue-chart-3"

const connected = ref(false)
const snapshot = ref({bids: [], asks: []})
const chartData = ref(null)

const chartOptions = {
  responsive: true,
  plugins: {
    legend: {display: true},
    tooltip: {mode: "index"}
  },
  scales: {
    x: {title: {display: true, text: "Price (EUR)"}},
    y: {title: {display: true, text: "Volume (BTC)"}}
  }
}

onMounted(async () => {
  const hub = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5284/orderbook") // ✅ update if different
      .withAutomaticReconnect()
      .build()

  hub.on("Snapshot", (data) => {
    snapshot.value = data
    updateChart(data)
  })

  try {
    await hub.start()
    connected.value = true
  } catch (err) {
    console.error("Error connecting to hub:", err)
  }
})

function updateChart(data) {
  const asks = data.asks.slice(0, 15)
  const bids = data.bids.slice(0, 15)

  chartData.value = {
    labels: [...asks.map(a => a.price), ...bids.map(b => b.price)],
    datasets: [
      {
        label: "Asks",
        data: [
          ...asks.map(a => a.amount),
          ...new Array(bids.length).fill(null)
        ],
        backgroundColor: "rgba(255, 99, 132, 0.7)"
      },
      {
        label: "Bids",
        data: [
          ...new Array(asks.length).fill(null),
          ...bids.map(b => b.amount)
        ],
        backgroundColor: "rgba(75, 192, 75, 0.7)"
      }
    ]
  }
}
</script>

<style scoped>
.orderbook {
  font-family: sans-serif;
  max-width: 900px;
  margin: auto;
}

.chart {
  margin: 2rem 0;
}

.flex {
  display: flex;
  justify-content: space-between;
  gap: 2rem;
}

.list {
  flex: 1;
}
</style>
