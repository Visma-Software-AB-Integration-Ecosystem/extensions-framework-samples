<script setup lang="ts">
import { RouterLink, RouterView, useRoute } from 'vue-router'

const route = useRoute()
</script>

<template>
  <div>
    <nav class="nav-container">
      <RouterLink
        :class="{ 'router-link-active': route.fullPath.includes('apirequests') }"
        :to="{ name: 'apirequests' }"
        >API Requests</RouterLink
      >
      <RouterLink
        :class="{ 'router-link-active': route.fullPath.includes('windowrequests') }"
        :to="{ name: 'windowrequests' }"
        >Window Requests</RouterLink
      >
    </nav>
  </div>
  <RouterView />
</template>
