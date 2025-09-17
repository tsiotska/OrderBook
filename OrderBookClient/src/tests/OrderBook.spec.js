import { mount } from "@vue/test-utils"
import { describe, it, expect, vi } from "vitest"
import OrderBook from "../components/OrderBook.vue"

vi.mock("vue-chart-3", () => ({
  BarChart: {
    name: "BarChart",
    template: "<div class='mocked-bar-chart'></div>"
  }
}))

vi.mock("chart.js/auto", () => ({
  default: {}
}))

describe("OrderBook.vue", () => {
  it("renders asks and bids from snapshot", async () => {
    const wrapper = mount(OrderBook)

    wrapper.vm.snapshot = {
      asks: [
        { price: 10000, amount: 0.5 },
        { price: 10100, amount: 0.2 }
      ],
      bids: [
        { price: 9900, amount: 0.3 },
        { price: 9800, amount: 0.4 }
      ]
    }
    await wrapper.vm.$nextTick()

    const asksText = wrapper.find(".list:first-child").text()
    expect(asksText).toContain("0.5 BTC @ €10000")

    const bidsText = wrapper.find(".list:last-child").text()
    expect(bidsText).toContain("0.3 BTC @ €9900")
  })

  it("updates chart data when snapshot is set", () => {
    const wrapper = mount(OrderBook)

    wrapper.vm.updateChart({
      asks: [{ price: 10000, amount: 0.5 }],
      bids: [{ price: 9900, amount: 0.3 }]
    })

    expect(wrapper.vm.chartData.labels).toContain(10000)
    expect(wrapper.vm.chartData.labels).toContain(9900)
    expect(wrapper.vm.chartData.datasets[0].data[0]).toBe(0.5)
    expect(wrapper.vm.chartData.datasets[1].data[1]).toBe(0.3)
  })

  it("renders chart when data is set", async () => {
    const wrapper = mount(OrderBook)

    wrapper.vm.updateChart({
      asks: [{ price: 10000, amount: 0.5 }],
      bids: [{ price: 9900, amount: 0.3 }]
    })
    await wrapper.vm.$nextTick()

    expect(wrapper.find(".mocked-bar-chart").exists()).toBe(true)
  })
})
