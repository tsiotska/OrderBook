<template>
  <div class="flex mx-auto p-8 gap-4">
    <div class="flex-3">
      <TradingView/>
    </div>
    <div class="flex flex-col flex-1">
      <OrderBook/>
      <QuoteForm/>
    </div>
  </div>
</template>

<script setup>
import OrderBook from './components/OrderBook.vue'
import QuoteForm from "@/components/QuoteForm.vue"
import TradingView from "@/components/TradingView.vue";
</script>