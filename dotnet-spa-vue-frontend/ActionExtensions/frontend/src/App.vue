<script setup lang="ts">
import { ref, watch, onMounted } from 'vue'
import { ApiCommands, CustomerCommands, WindowCommands, ErrorCommands } from '@/constants'
import type {
  Request,
  ResponseCmdRequest,
  UrlRequest,
  ContentBodyRequest,
  EmptyBodyRequest,
  OpenRequest,
  InitRequest
} from '@/requestTypes'
import type {
  InitMessage,
  PostResponse,
  GetResponse,
  PutResponse,
  DeleteResponse,
  InitResponse,
  ErrorResponse
} from '@/responseTypes'

const request = ref<Request>({
  command: ApiCommands.API_INIT,
  responsecmd: WindowCommands.INIT,
  caption: 'Action Extension',
  width: 1440,
  height: 900
})

const popupClosed = ref(false)
const messageTarget = ref('')

const requestData = ref(JSON.stringify(request.value, null, 2))
const responseData = ref('The response will be displayed here after a request has been sent')

const commands = [...Object.values(CustomerCommands), ...Object.values(WindowCommands)]

onMounted(() => {
  window.open(
    '/vismaconnect/popuplogin',
    '_blank',
    'location=0,menubar=0,resizable=0,titlebar=0,toolbar=0,scrollbars=0,status=0'
  )
  console.info('Running popup login...')
})

// Handle popup authentication window
interface CustomWindow extends Window {
  popupConfirmation: () => void
}
;(window as unknown as CustomWindow).popupConfirmation = () => (popupClosed.value = true)

watch(
  request,
  () => {
    requestData.value = JSON.stringify(request.value, null, 2)
  },
  { deep: true }
)

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
  window.parent.postMessage(JSON.parse(JSON.stringify(request.value)), messageTarget.value)
}

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
    case ApiCommands.API_INIT:
      {
        const response = message as InitMessage

        // Message target is required by all the commands so this is set when the application is initialized
        messageTarget.value = response.messageTarget

        responseData.value = JSON.stringify(response, null, 2)

        // Send an INIT request to resize the window
        sendMessage()
      }
      break
    case CustomerCommands.CUSTOMER_CREATED:
      {
        const response = message as PostResponse

        responseData.value = JSON.stringify(response, null, 2)
      }
      break
    case CustomerCommands.CUSTOMER_FETCHED:
      {
        const response = message as GetResponse

        responseData.value = JSON.stringify(response, null, 2)
      }
      break
    case CustomerCommands.CUSTOMER_UPDATED:
      {
        const response = message as PutResponse

        responseData.value = JSON.stringify(response, null, 2)
      }
      break
    case CustomerCommands.CUSTOMER_DELETED:
      {
        const response = message as DeleteResponse

        responseData.value = JSON.stringify(response, null, 2)
      }
      break
    // This is the INIT message response that is received after sending an INIT request
    case WindowCommands.INIT:
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

function commandChanged(target: EventTarget | null) {
  const command = (target as HTMLSelectElement).value
  switch (command) {
    case ApiCommands.API_GET:
    case CustomerCommands.CUSTOMER_FETCHED:
      request.value = {
        command: ApiCommands.API_GET,
        url: 'api/v1/customer?pagesize=1&pagenumber=1',
        responsecmd: CustomerCommands.CUSTOMER_FETCHED
      } as EmptyBodyRequest
      break
    case ApiCommands.API_POST:
    case CustomerCommands.CUSTOMER_CREATED:
      request.value = {
        command: ApiCommands.API_POST,
        url: 'api/v1/customer/{customerId}',
        content: '{\n\n}',
        responsecmd: CustomerCommands.CUSTOMER_CREATED
      } as ContentBodyRequest
      break
    case ApiCommands.API_PUT:
    case CustomerCommands.CUSTOMER_UPDATED:
      request.value = {
        command: ApiCommands.API_PUT,
        url: 'api/v1/customer/{customerId}',
        content: '{\n\n}',
        responsecmd: CustomerCommands.CUSTOMER_UPDATED
      } as ContentBodyRequest
      break
    case ApiCommands.API_DELETE:
    case CustomerCommands.CUSTOMER_DELETED:
      request.value = {
        command: ApiCommands.API_DELETE,
        url: 'api/v1/customer/{customerId}',
        responsecmd: CustomerCommands.CUSTOMER_DELETED
      } as EmptyBodyRequest
      break
    case ApiCommands.API_COMMAND_OPEN:
      request.value = {
        command: ApiCommands.API_COMMAND_OPEN,
        primaryKey: 'customerId'
      } as OpenRequest
      break
    case ApiCommands.API_COMMAND_REFRESH:
      request.value = {
        command: ApiCommands.API_COMMAND_REFRESH
      } as Request
      break
    case ApiCommands.API_COMMAND_CANCEL:
      request.value = {
        command: ApiCommands.API_COMMAND_CANCEL
      } as Request
      break
    case ApiCommands.API_INIT:
      request.value = {
        command: ApiCommands.API_INIT,
        responsecmd: WindowCommands.INIT,
        caption: 'Action Extension',
        width: 1440,
        height: 900
      } as InitRequest
      break
    default:
      // TODO: Handle default cases
      break
  }
}
</script>

<template>
  <header>
    <h2>Visma Net Extensions Framework - Action Extensions</h2>
  </header>

  <main v-if="!popupClosed">
    <div class="loading-overlay">
      <h2>Waiting for popup authentication...</h2>
    </div>
  </main>
  <main v-else>
    <div class="command-form">
      <div class="label-form">
        <label for="select-request-command">Request command</label>
        <select
          id="select-request-command"
          v-model="request.command"
          @change="commandChanged($event.target)"
        >
          <option v-for="(command, index) in ApiCommands" :key="index" :value="command">
            {{ command }}
          </option>
        </select>
      </div>

      <div v-if="(request as UrlRequest).url" class="label-form">
        <label for="url">Endpoint (url)</label>
        <input id="url" type="text" v-model="(request as UrlRequest).url" />
      </div>

      <div v-if="(request as ResponseCmdRequest).responsecmd" class="label-form">
        <label for="select-response-command">Response command</label>
        <select
          id="select-response-command"
          v-model="(request as ResponseCmdRequest).responsecmd"
          @change="commandChanged($event.target)"
        >
          <option v-for="(command, index) in commands" :key="index" :value="command">
            {{ command }}
          </option>
        </select>
      </div>

      <div v-if="(request as OpenRequest).primaryKey" class="label-form">
        <label for="url">Primary key</label>
        <input id="url" type="text" v-model="(request as OpenRequest).primaryKey" />
      </div>

      <div v-if="(request as InitRequest).caption" class="label-form">
        <label for="url">Caption</label>
        <input id="url" type="text" v-model="(request as InitRequest).caption" />
      </div>

      <div v-if="(request as InitRequest).width" class="label-form">
        <label for="url">Width (px)</label>
        <input id="url" type="text" v-model="(request as InitRequest).width" />
      </div>

      <div v-if="(request as InitRequest).height" class="label-form">
        <label for="url">Height (px)</label>
        <input id="url" type="text" v-model="(request as InitRequest).height" />
      </div>

      <button @click="sendMessage()">Send Message</button>
    </div>

    <div v-if="(request as ContentBodyRequest).content" class="text-area-form">
      <label for="url">Content Body</label>
      <textarea class="text-area" v-model="(request as ContentBodyRequest).content"></textarea>
    </div>

    <h2>Result</h2>
    <div class="text-areas-container">
      <div class="text-area-form">
        <label>Request</label>
        <textarea readonly class="text-area" v-model="requestData"></textarea>
      </div>
      <div class="text-area-form">
        <label>Response</label>
        <textarea readonly class="text-area" v-model="responseData"></textarea>
      </div>
    </div>
  </main>
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
.command-form {
  display: flex;
  flex-direction: column;
  align-items: start;
  justify-content: space-between;
  margin-bottom: 2rem;
}
.label-form {
  display: flex;
  flex-direction: column;
  margin-bottom: 0.5rem;
  width: 20rem;
}

.text-areas-container {
  display: flex;
  flex-direction: row;
  justify-content: space-around;
}

.text-area-form {
  margin: 1rem;
  display: flex;
  flex-direction: column;
}

.text-area {
  resize: none;
  width: 30rem;
  height: 30rem;
}

header {
  padding: 2rem;
  text-align: center;
}

main {
  display: flex;
  flex-direction: column;
  align-items: center;
}
</style>
