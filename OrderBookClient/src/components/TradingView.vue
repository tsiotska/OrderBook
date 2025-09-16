<template>
  <div class="h-full w-full">
    <div id="tradingview_chart" class="h-full"></div>
  </div>
</template>

<script setup>
import {onMounted} from "vue"

onMounted(() => {
  const script = document.createElement("script")
  script.src = "https://s3.tradingview.com/tv.js"
  script.async = true
  script.onload = () => {
    new window.TradingView.widget({
      autosize: true,
      symbol: "BITSTAMP:BTCEUR",
      interval: "15",
      timezone: "Etc/UTC",
      theme: "dark",
      style: "1",
      locale: "en",
      container_id: "tradingview_chart"
    })
  }
  document.body.appendChild(script)
})
</script>
