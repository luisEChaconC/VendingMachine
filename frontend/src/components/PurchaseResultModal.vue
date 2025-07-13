<template>
  <div v-if="show" class="modal fade show d-block" tabindex="-1" style="background-color: rgba(0,0,0,0.5);" @click="closeModal">
    <div class="modal-dialog modal-dialog-centered" @click.stop>
      <div class="modal-content">
        <div class="modal-header" :class="result.success ? 'bg-success text-white' : 'bg-danger text-white'">
          <h5 class="modal-title">{{ result.success ? 'Compra Exitosa' : 'Error en la Compra' }}</h5>
          <button type="button" class="btn-close" @click="closeModal" :class="result.success ? 'btn-close-white' : 'btn-close-white'"></button>
        </div>
        <div class="modal-body">
          <div v-if="result.success">
            <p><strong>Su vuelto es de {{ formatPrice(result.changeAmount) }}.</strong></p>
            <div v-if="result.changeAmount > 0">
              <p><strong>Desglose:</strong></p>
              <ul>
                <li v-for="item in filteredChangeBreakdown" :key="item.denomination">
                  {{ item.quantity }} {{ getDenominationText(item.denomination) }}
                </li>
              </ul>
            </div>
            <div v-else>
              <p>No hay vuelto que devolver.</p>
            </div>
          </div>
          <div v-else>
            <p><strong>{{ result.message }}</strong></p>
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-primary" @click="closeModal">Cerrar</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'PurchaseResultModal',
  props: {
    show: {
      type: Boolean,
      default: false
    },
    result: {
      type: Object,
      default: () => ({})
    }
  },
  data() {
    return {
      denominationMap: {
        'Bill1000': 1000,
        'Coin500': 500,
        'Coin100': 100,
        'Coin50': 50,
        'Coin25': 25
      }
    };
  },
  computed: {
    filteredChangeBreakdown() {
      if (!this.result || !this.result.changeBreakdown) {
        return [];
      }
      return Object.entries(this.result.changeBreakdown)
        .filter(([, quantity]) => quantity > 0)
        .map(([denomination, quantity]) => ({ 
          denomination: denomination,
          quantity: quantity,
          value: this.denominationMap[denomination] || parseInt(denomination)
        }));
    }
  },
  mounted() {
    document.addEventListener('keydown', this.handleKeydown);
  },
  beforeUnmount() {
    document.removeEventListener('keydown', this.handleKeydown);
  },
  methods: {
    closeModal() {
      this.$emit('close');
    },
    handleKeydown(event) {
      if (event.key === 'Escape' && this.show) {
        this.closeModal();
      }
    },
    formatPrice(price) {
      return `₵${price.toFixed(0)}`;
    },
    getDenominationText(denomination) {
      const denom = this.denominationMap[denomination] || parseInt(denomination);
      if (denom === 1000) {
        return 'billete de 1000';
      } else {
        return `moneda de ${denom}`;
      }
    }
  }
}
</script>

<style scoped>
</style> 