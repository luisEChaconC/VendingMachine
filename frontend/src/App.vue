<template>
  <div id="app" class="min-vh-100 bg-light">
    <VendingMachine />
  </div>
</template>

<script>
import VendingMachine from './views/VendingMachine.vue'

export default {
  name: 'App',
  components: {
    VendingMachine
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
}
</style>
