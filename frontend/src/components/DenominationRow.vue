<template>
  <tr>
    <td>{{ denomination }}</td>
    <td class="text-center">
      <div class="quantity-selector">
        <span class="quantity-display">{{ quantity }}</span>
        <div class="quantity-arrows">
          <button 
            class="btn btn-outline-secondary arrow-btn arrow-up" 
            type="button" 
            @click="incrementQuantity"
          >
            ▲
          </button>
          <button 
            class="btn btn-outline-secondary arrow-btn arrow-down" 
            type="button" 
            @click="decrementQuantity"
            :disabled="quantity <= 0"
          >
            ▼
          </button>
        </div>
      </div>
    </td>
  </tr>
</template>

<script>
export default {
  name: 'DenominationRow',
  props: {
    denomination: {
      type: Number,
      required: true
    },
    quantity: {
      type: Number,
      default: 0
    }
  },
  methods: {
    incrementQuantity() {
      this.$emit('quantity-changed', this.denomination, this.quantity + 1);
    },
    decrementQuantity() {
      if (this.quantity > 0) {
        this.$emit('quantity-changed', this.denomination, this.quantity - 1);
      }
    }
  }
}
</script>

<style scoped>
.quantity-selector {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  justify-content: center;
}

.quantity-display {
  display: inline-block;
  min-width: 20px;
  padding: 4px 8px;
  font-size: 14px;
  text-align: center;
  border: 1px solid #ced4da;
  border-radius: 4px;
  background-color: #fff;
}

.quantity-arrows {
  display: flex;
  flex-direction: column;
  gap: 1px;
}

.arrow-btn {
  width: 20px;
  height: 16px;
  padding: 0;
  font-size: 10px;
  line-height: 1;
  border: 1px solid #ced4da;
  background-color: #f8f9fa;
  color: #6c757d;
  display: flex;
  align-items: center;
  justify-content: center;
}

.arrow-up {
  border-radius: 3px 3px 0 0;
}

.arrow-down {
  border-radius: 0 0 3px 3px;
}

.arrow-btn:hover:not(:disabled) {
  background-color: #e9ecef;
  color: #495057;
}

.arrow-btn:disabled {
  background-color: #f8f9fa;
  color: #adb5bd;
  cursor: not-allowed;
}
</style> 