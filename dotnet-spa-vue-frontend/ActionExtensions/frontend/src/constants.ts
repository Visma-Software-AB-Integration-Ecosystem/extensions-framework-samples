export const ApiCommands = {
  API_GET: 'apiget',
  API_DELETE: 'apidelete',
  API_POST: 'apipost',
  API_PUT: 'apiput',
  API_COMMAND_OPEN: 'open',
  API_COMMAND_REFRESH: 'refresh',
  API_COMMAND_CANCEL: 'cancel',
  API_INIT: 'init'
} as const

export type ApiCommandTypes = (typeof ApiCommands)[keyof typeof ApiCommands]

export const WindowCommands = {
  INIT: 'initCommand'
} as const

export type WindowCommandTypes = (typeof WindowCommands)[keyof typeof WindowCommands]

export const ErrorCommands = {
  ERROR: 'error',
  RESPONSECMD: '<responsecmd>'
} as const

export const CustomerCommands = {
  CUSTOMER_CREATED: 'customerCreated',
  CUSTOMER_FETCHED: 'customerFetched',
  CUSTOMER_UPDATED: 'customerUpdated',
  CUSTOMER_DELETED: 'customerDeleted'
} as const

export type CustomerCommandTypes = (typeof CustomerCommands)[keyof typeof CustomerCommands]

// TODO: Add commands for other entities here
