<script setup lang="ts">
import { RouterView, RouterLink, useRoute } from 'vue-router'

const route = useRoute()
</script>

<template>
  <div>
    <nav class="nav-container">
      <RouterLink
        :class="{ 'router-link-active': route.fullPath.includes('supplierclasses') }"
        :to="{ name: 'supplierclasses' }"
        >GET Supplier Classes</RouterLink
      >
      <RouterLink
        :class="{ 'router-link-active': route.fullPath.includes('supplierinvoice') }"
        :to="{ name: 'supplierinvoice' }"
        >POST Supplier Invoice</RouterLink
      >
    </nav>
  </div>
  <RouterView />
</template>
