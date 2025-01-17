/** Used to be able to run application locally */
export const BASEURL = process.env.NODE_ENV === 'development' ? 'https://localhost:0000' : ''

/** The different commands for Visma Net Extensions */
export const WindowCommands = {
  /** Opens a new page in the document of the host */
  COMMAND_OPEN: 'open',
  /** Refreshes the page in the document of the host */
  COMMAND_REFRESH: 'refresh',
  /** Closes the extension window */
  COMMAND_CANCEL: 'cancel',
  /** Informs the host to do initialization based on provided caption, height, width */
  COMMAND_INIT: 'init'
} as const

export type ApiCommandTypes = (typeof WindowCommands)[keyof typeof WindowCommands]

/** Used when receiving responses after sending requests to the host window */
export const WindowResponseCommands = {
  INIT: 'initCommand'
} as const

export type WindowCommandTypes = (typeof WindowCommands)[keyof typeof WindowCommands]

/** Commands for handling errors */
export const ErrorCommands = {
  ERROR: 'error',
  RESPONSECMD: '<responsecmd>'
} as const
