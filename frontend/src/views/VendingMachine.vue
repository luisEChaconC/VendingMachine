<template>
  <div class="container mt-4">
    <div class="row justify-content-center">
      <div class="col-md-10">
        <div class="card shadow">
          <div class="card-body p-0">
            <table class="table table-striped table-hover mb-0">
              <thead class="table-light">
                <tr>
                  <th scope="col">Nombre</th>
                  <th scope="col" class="text-center">Precio</th>
                  <th scope="col" class="text-center">Unidades Disponibles</th>
                  <th scope="col" class="text-center">Cantidad</th>
                </tr>
              </thead>
              <tbody>
                <ProductRow 
                  v-for="product in products" 
                  :key="product.id" 
                  :product="product"
                  :quantity="quantities[product.id] || 0"
                  @quantity-changed="updateQuantity"
                />
              </tbody>
            </table>
          </div>
          <div class="card-footer bg-light">
            <div class="row">
              <div class="col-md-6">
                <strong>Costo Total: {{ formatPrice(totalCost) }}</strong>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import ProductRow from '../components/ProductRow.vue';

export default {
  name: 'VendingMachine',
  components: {
    ProductRow
  },
  data() {
    return {
      products: [],
      quantities: {}
    };
  },
  computed: {
    totalCost() {
      let total = 0;
      for (const product of this.products) {
        const quantity = this.quantities[product.id] || 0;
        total += product.price * quantity;
      }
      return total;
    }
  },
  async mounted() {
    await this.fetchProducts();
  },
  methods: {
    async fetchProducts() {
      try {
        const response = await axios.get('/api/products');
        this.products = response.data;
      } catch (error) {
        console.error('Error fetching products:', error);
      }
    },
    updateQuantity(productId, newQuantity) {
      this.quantities[productId] = newQuantity;
    },
    formatPrice(price) {
      return `₵${price.toFixed(0)}`;
    }
  }
};
</script>

<style scoped>
.table th {
  font-weight: 600;
}
</style> 