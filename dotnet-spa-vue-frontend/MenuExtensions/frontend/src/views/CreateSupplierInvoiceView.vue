<script setup lang="ts">
import { BASEURL } from '@/constants'
import type {
  CreateSupplierInvoiceModel,
  CreateSupplierInvoiceLineModel,
} from '@/types/CreateSupplierInvoiceModel'
import { ref } from 'vue'

const invoiceInputData = ref<CreateSupplierInvoiceModel>({
  supplierNumber: { value: '' },
  supplierReference: { value: '' },
  invoiceLines: [
    {
      lineNumber: { value: 1 },
      inventoryNumber: { value: '' },
    },
  ],
})
const invoiceSubmissionStatus = ref('')

function addLine() {
  invoiceInputData.value.invoiceLines.push({
    lineNumber: { value: invoiceInputData.value.invoiceLines.length + 1 },
    inventoryNumber: { value: '' },
  })
}

function removeLine(lineNumber: number) {
  const newInvoiceLines: CreateSupplierInvoiceLineModel[] = []
  let removedLinePassed = false
  for (const line of invoiceInputData.value.invoiceLines) {
    if (line.lineNumber.value === lineNumber) {
      removedLinePassed = true
    } else {
      if (removedLinePassed) {
        line.lineNumber.value--
      }
      newInvoiceLines.push(line)
    }
  }
  invoiceInputData.value.invoiceLines = newInvoiceLines
}

async function submitInvoice() {
  try {
    invoiceSubmissionStatus.value = 'Submitting invoice...'
    await fetch(`${BASEURL}/vismanet/supplierinvoice`, {
      method: 'POST',
      credentials: 'include',
      body: JSON.stringify(invoiceInputData.value),
      headers: new Headers({
        'Content-Type': 'application/json',
      }),
    })

    invoiceSubmissionStatus.value = 'Invoice submitted successfully'
  } catch (error) {
    console.error('Error fetching supplier classes:', error)
    invoiceSubmissionStatus.value = 'Error submitting invoice'
  }
}
</script>

<template>
  <div class="container">
    <form role="form" @submit.prevent="submitInvoice">
      <div class="form-group">
        <h1>Create Supplier Invoice</h1>

        <label for="supplierNumber">Supplier number:</label>
        <input
          type="text"
          class="form-control"
          placeholder="12345"
          name="textinput"
          id="supplierNumber"
          v-model="invoiceInputData.supplierNumber.value"
        />

        <label for="supplierReference">Supplier reference:</label>
        <input
          type="text"
          class="form-control"
          placeholder="Reference"
          name="textinput"
          id="supplierReference"
          v-model="invoiceInputData.supplierReference.value"
        />

        <h2>Invoice lines</h2>
        <template v-for="line in invoiceInputData.invoiceLines" :key="line.lineNumber.value">
          <div class="line-item-header">
            <label for="inventoryNumber-{{ line.lineNumber }}"
              >Line {{ line.lineNumber.value }} Item ID:</label
            >
            <button type="button" class="btn btn-small" @click="removeLine(line.lineNumber.value)">
              Remove line
            </button>
          </div>
          <input
            type="text"
            class="form-control"
            placeholder="A12345"
            name="textinput"
            id="inventoryNumber-{{ line.lineNumber }}"
            v-model="line.inventoryNumber.value"
          />
        </template>

        <div>
          <button type="button" class="btn btn-primary" @click="addLine()">Add new line</button>
        </div>

        <div class="text-align-right">
          <button type="submit" class="btn btn-primary">Submit</button>
        </div>
      </div>
    </form>

    <div>
      <h2>{{ invoiceSubmissionStatus }}</h2>
    </div>
  </div>
</template>

<style scoped>
.line-item-header {
  display: flex;
  justify-content: space-between;
}
</style>
