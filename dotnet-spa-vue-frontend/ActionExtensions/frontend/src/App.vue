<script setup lang="ts">
import { onMounted, ref, watch, provide } from 'vue'
import { WindowResponseCommands, WindowCommands, ErrorCommands } from '@/constants'
import type { WindowRequest } from '@/types/requestTypes'
import type { InitMessage, InitResponse, ErrorResponse } from '@/types/responseTypes'

const request = ref<WindowRequest>({
  command: WindowCommands.COMMAND_INIT,
  responsecmd: WindowResponseCommands.INIT,
  caption: 'Action Extension',
  width: 1440,
  height: 900
})

const messageTarget = ref('')
// Use provide to make messageTarget available to children (mainly WindowRequestsView)
provide('messageTarget', messageTarget)

// Tenant ID is required to verify that the user is authenticated to Visma Connect
const tenantId = ref('')
const isLoading = ref(true)
const primaryKey = ref('')

const responseData = ref('The response will be displayed here after a request has been sent')

window.addEventListener(
  'message',
  async (event) => {
    // Validate message, then process it
    if (event.data && event.data.command !== undefined) {
      console.info('Received message', event.data)
      await processIncomingMessage(event.data)
    }
  },
  false
)

function sendMessage() {
  console.info('Sending message', request.value)
  console.log('messageTarget.value: ', messageTarget.value)
  // Check if messageTarget.value is valid
  if (!messageTarget.value) {
    console.error('Invalid message target:', messageTarget.value)
    return // Exit if the target is invalid
  }
  window.parent.postMessage(JSON.parse(JSON.stringify(request.value)), messageTarget.value)
}

// Run popup login window which is required to authenticate the user
function runPopupLogin() {
  window.open(
    '/vismaconnect/popuplogin?tenantid=' + tenantId.value,
    '_blank',
    'location=0,menubar=0,resizable=0,titlebar=0,toolbar=0,scrollbars=0,status=0'
  )
  console.info('Running popup login...')
}

// Set loading to false when the popup login window is closed
interface CustomWindow extends Window {
  popupConfirmation: () => void
}
;(window as unknown as CustomWindow).popupConfirmation = () => (isLoading.value = false)

async function processIncomingMessage(message: any) {
  switch (message.command) {
    case ErrorCommands.ERROR:
    case ErrorCommands.RESPONSECMD:
      {
        const response = message as ErrorResponse

        responseData.value = JSON.stringify(response, null, 2)
      }
      break
    // This is the INIT message that is received when the application is opened in Visma Net
    case WindowCommands.COMMAND_INIT:
      {
        const response = message as InitMessage

        // Message target is required by all the commands so this is set when the application is initialized
        messageTarget.value = response.messageTarget

        // Save Tenant ID to use for authentication call to back-end
        tenantId.value = response.tenantId

        console.info('Tenant ID:', tenantId.value)

        // Run popup login to authenticate the user
        // This has to be a popup because of the same-origin policy of Visma Net
        runPopupLogin()

        primaryKey.value = response.primaryKey

        responseData.value = JSON.stringify(response, null, 2)

        // Send an INIT request to resize the window
        sendMessage()
      }
      break

    // This is the INIT message response that is received after sending an INIT request
    case WindowResponseCommands.INIT:
      {
        const response = message as InitResponse

        responseData.value = JSON.stringify(response, null, 2)
      }
      break
    default:
      // TODO: Handle default cases
      break
  }
}

// This is used to initialize the application when running locally
onMounted(() => {
  if (window.location.origin.includes('localhost')) {
    processIncomingMessage({
      command: WindowCommands.COMMAND_INIT,
      messageTarget: window.parent,
      primaryKey: 'supplier',
      secondaryKey: 'null',
      companyId: '0000000',
      user: 'firstname.lastname@visma.com',
      language: 'en',
      tenantId: 'b6ef7e99-69be-11ef-b082-0a07a6e6b5cd',
      branchId: '1',
      userChangesPresent: false
    })
  }
})
</script>

<template>
  <div v-if="isLoading" class="loading-overlay">
    <h1>Loading...</h1>
  </div>
  <RouterView />
</template>

<style scoped>
.loading-overlay {
  padding-top: 20rem;
  width: 100%;
  height: 100vh;
  z-index: 999;
  display: flex;
  align-items: center;
  flex-direction: column;
}
</style>
