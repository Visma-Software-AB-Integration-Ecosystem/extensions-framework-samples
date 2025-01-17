import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import ApiRequestsView from '@/views/ApiRequestsView.vue'
import WindowRequestsView from '@/views/WindowRequestsView.vue'
import SupplierClassesView from '@/views/SupplierClassesView.vue'
import SupplierInvoiceView from '@/views/CreateSupplierInvoiceView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
      children: [
        {
          path: 'apirequests',
          name: 'apirequests',
          component: ApiRequestsView,
          children: [
            { path: 'supplierclasses', name: 'supplierclasses', component: SupplierClassesView },
            { path: 'supplierinvoice', name: 'supplierinvoice', component: SupplierInvoiceView }
          ]
        },
        {
          path: 'windowrequests',
          name: 'windowrequests',
          component: WindowRequestsView
        }
      ]
    }
  ]
})

export default router
