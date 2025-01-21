export type WindowRequest = OpenRequest | RefreshOrCancelRequest | InitRequest

export type OpenRequest = {
  /** Informs host page to open new document/UI in case of creating new customer/supplier/supplier invoice */
  command: string
  /** ID of the host page document */
  primaryKey: string
}

export type RefreshOrCancelRequest = {
  /** Informs host page to refresh/reload current document/UI OR closes screen/panel where service page is loaded */
  command: string
}

export type InitRequest = {
  /** Informs the host to do initialization based on provided caption, height, width */
  command: string
  /** Host sends a response message, the messages "command" property will have this value. This allows service page to check this incoming "response" message. */
  responsecmd: string
  /** Caption to be set on host panel */
  caption: string
  /** Height to be set on host panel */
  height: number
  /** Width to be set on host panel */
  width: number
}
