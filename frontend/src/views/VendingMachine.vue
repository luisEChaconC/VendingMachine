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

        <div class="card shadow mt-4">
          <div class="card-header bg-light">
            <h5 class="mb-0">Pago</h5>
          </div>
          <div class="card-body p-0">
            <table class="table table-striped mb-0">
              <tbody>
                <DenominationRow
                  v-for="denomination in denominations"
                  :key="denomination"
                  :denomination="denomination"
                  :quantity="payment[denomination] || 0"
                  @quantity-changed="updatePayment"
                />
              </tbody>
            </table>
          </div>
          <div class="card-footer">
            <button 
              class="btn btn-secondary btn-lg"
              @click="makePurchase"
              :disabled="!canPurchase"
            >
              Comprar
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import ProductRow from '../components/ProductRow.vue';
import DenominationRow from '../components/DenominationRow.vue';

export default {
  name: 'VendingMachine',
  components: {
    ProductRow,
    DenominationRow
  },
  data() {
    return {
      products: [],
      quantities: {},
      payment: {},
      denominations: [1000, 500, 100, 50, 25]
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
    },
    totalPaid() {
      let total = 0;
      for (const denomination of this.denominations) {
        const quantity = this.payment[denomination] || 0;
        total += denomination * quantity;
      }
      return total;
    },
    canPurchase() {
      return this.totalCost > 0 && this.totalPaid >= this.totalCost;
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
    updatePayment(denomination, newQuantity) {
      this.payment[denomination] = newQuantity;
    },
    async makePurchase() {
      try {
        const purchaseItems = [];
        for (const product of this.products) {
          const quantity = this.quantities[product.id] || 0;
          if (quantity > 0) {
            purchaseItems.push({
              productId: product.id,
              quantity: quantity
            });
          }
        }

        const paymentDenominations = {};
        for (const denomination of this.denominations) {
          const quantity = this.payment[denomination] || 0;
          if (quantity > 0) {
            paymentDenominations[denomination] = quantity;
          }
        }

        const purchaseRequest = {
          purchaseItems: purchaseItems,
          payment: {
            denominations: paymentDenominations
          }
        };

        const response = await axios.post('/api/purchase', purchaseRequest);
        
        console.log('Purchase successful:', response.data);
        alert('Compra realizada exitosamente!');
        
        this.quantities = {};
        this.payment = {};
        await this.fetchProducts();
        
      } catch (error) {
        console.error('Error making purchase:', error);
        if (error.response && error.response.data) {
          alert(`Error: ${error.response.data.message}`);
        } else {
          alert('Error al procesar la compra');
        }
      }
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