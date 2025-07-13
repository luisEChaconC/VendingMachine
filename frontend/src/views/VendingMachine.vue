<template>
  <div class="container mt-4">
    <div class="row justify-content-center">
      <div class="col-md-8">
        <div class="card shadow">
          <div class="card-body p-0">
            <table class="table table-striped table-hover mb-0">
              <thead class="table-light">
                <tr>
                  <th scope="col">Nombre</th>
                  <th scope="col" class="text-center">Precio</th>
                  <th scope="col" class="text-center">Unidades Disponibles</th>
                </tr>
              </thead>
              <tbody>
                <ProductRow 
                  v-for="product in products" 
                  :key="product.id" 
                  :product="product"
                />
              </tbody>
            </table>
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
      products: []
    };
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
    }
  }
};
</script>

<style scoped>
.table th {
  font-weight: 600;
}
</style> 