/** Used to be able to run application locally */
export const BASEURL = process.env.NODE_ENV === 'development' ? 'https://localhost:0000' : ''

/** The different commands for Visma Net Extensions */
export const WindowCommands = {
  /** Opens a new page in the document of the host */
  COMMAND_OPEN: 'open',
  /** Refreshes the page in the document of the host */
  COMMAND_REFRESH: 'refresh',
  /** This is used when handling the INIT message that is sent from Visma Net when the extension starts */
  COMMAND_INIT: 'init'
} as const

/** Commands for handling errors */
export const ErrorCommands = {
  ERROR: 'error',
  RESPONSECMD: '<responsecmd>'
} as const
