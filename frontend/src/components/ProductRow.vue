<template>
  <tr>
    <td>{{ product.name }}</td>
    <td class="text-center">{{ formatPrice(product.price) }}</td>
    <td class="text-center">{{ product.stock }}</td>
  </tr>
</template>

<script>
export default {
  name: 'ProductRow',
  props: {
    product: {
      type: Object,
      required: true
    }
  },
  methods: {
    formatPrice(price) {
      return `₵ ${price.toFixed(0)}`;
    }
  }
}
</script>

<style scoped>
</style> 