<script setup lang="ts">
import { ref, watch, inject, type Ref } from 'vue'
import { WindowCommands } from '@/constants'
import type { WindowRequest, OpenRequest } from '@/types/requestTypes'

const request = ref<WindowRequest>({
  command: WindowCommands.COMMAND_REFRESH
})

const messageTarget = inject('messageTarget') as Ref<string>

const requestData = ref(JSON.stringify(request.value, null, 2))
const responseData = inject('responseData') as Ref<string>

watch(
  request,
  () => {
    requestData.value = JSON.stringify(request.value, null, 2)
  },
  // deep: true is required to watch changes in nested objects
  { deep: true }
)

function sendMessage() {
  window.parent.postMessage(JSON.parse(JSON.stringify(request.value)), messageTarget.value)
}

function commandChanged(target: EventTarget | null) {
  const command = (target as HTMLSelectElement).value
  switch (command) {
    case WindowCommands.COMMAND_OPEN:
      request.value = {
        command: WindowCommands.COMMAND_OPEN,
        primaryKey: 'customerId'
      } as OpenRequest
      break
    case WindowCommands.COMMAND_REFRESH:
      request.value = {
        command: WindowCommands.COMMAND_REFRESH
      } as WindowRequest
      break
    default:
      // TODO: Handle default cases
      break
  }
}
</script>

<template>
  <main class="container">
    <div class="command-form">
      <div class="label-form">
        <label for="select-request-command">Request command</label>
        <select
          id="select-request-command"
          v-model="request.command"
          @change="commandChanged($event.target)"
        >
          <option v-for="(command, index) in WindowCommands" :key="index" :value="command">
            {{ command }}
          </option>
        </select>
      </div>

      <div v-if="(request as OpenRequest).primaryKey" class="label-form">
        <label for="url">Primary key</label>
        <input id="url" type="text" v-model="(request as OpenRequest).primaryKey" />
      </div>

      <button @click="sendMessage()">Send Message</button>
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
</style>
