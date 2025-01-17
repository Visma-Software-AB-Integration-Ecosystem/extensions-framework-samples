<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { RouterView } from 'vue-router'
import { BASEURL } from '@/constants'
import type { SupplierClassModel } from '@/types/SupplierClassModel'
const supplierClasses = ref<SupplierClassModel[]>([])

async function fetchSupplierClasses() {
  try {
    const response = await fetch(`${BASEURL}/vismanet/supplierclass`, {
      credentials: 'include'
    })
    const data = await response.json()
    supplierClasses.value = data
  } catch (error) {
    console.error('Error fetching supplier classes:', error)
  }
}
onMounted(fetchSupplierClasses)
</script>

<template>
  <div class="container">
    <div v-if="supplierClasses.length === 0">
      <h1>Loading...</h1>
    </div>
    <div v-else>
      <h1>Supplier Classes</h1>
      <div>
        <ul v-for="supplierClass in supplierClasses" :key="supplierClass.id">
          <li>
            <p>{{ supplierClass.id }} - {{ supplierClass.description }}</p>
          </li>
        </ul>
      </div>
    </div>
  </div>

  <RouterView />
</template>

<style scoped>
.class-id-description {
  display: flex;
  flex-direction: row;
  justify-content: flex-start;
  align-items: center;
}
</style>
