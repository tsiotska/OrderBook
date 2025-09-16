<template>
  <div>
    <h3>Get Quote</h3>
    <input v-model.number="btcAmount" type="number" placeholder="BTC amount"/>
    <button @click="fetchQuote">Get Quote</button>

    <div v-if="quote">
      <p>Total EUR: €{{ quote.totalEur.toFixed(2) }}</p>
      <p>Average Price: €{{ quote.averagePrice.toFixed(2) }}</p>
    </div>
  </div>
</template>

<script setup>
import {ref} from "vue";

const btcAmount = ref(0.1)
const quote = ref(null)

async function fetchQuote() {
  try {
    const response = await fetch("http://localhost:5284/api/orderbook/quote", {
      method: "POST",
      headers: {"Content-Type": "application/json"},
      body: JSON.stringify({btcAmount: btcAmount.value})
    })
    if (!response.ok) throw new Error("Failed to fetch quote")
    quote.value = await response.json()
  } catch (err) {
    console.error(err)
    quote.value = null
  }
}
</script>