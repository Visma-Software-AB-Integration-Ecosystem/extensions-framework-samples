export type ResponseCmdRequest = ContentBodyRequest | EmptyBodyRequest | InitRequest
export type UrlRequest = ContentBodyRequest | EmptyBodyRequest
export type Request =
  | ContentBodyRequest
  | EmptyBodyRequest
  | OpenRequest
  | RefreshOrCancelRequest
  | InitRequest

export type ContentBodyRequest = {
  /** Informs host page to do a in-session API POST or API PUT operation */
  command: string
  /** Relative url of api endpoint, eg. api/v1/customer to create or update a customer entry */
  url: string
  /** The payload for the api post or put operation, eg. a customerUpdateDto */
  content: string
  /** Host sends a response message, the messages "command" property will have this value. This allows service page to check this incoming "response" message. */
  responsecmd: string
}

export type EmptyBodyRequest = {
  /** Informs host page to do a in-session API GET or API DELETE operation */
  command: string
  /** Relative url of api endpoint, eg. api/v1/customer?pagesize=1&pagenumber=1 for GET or api/v1/customer/abc for DELETE */
  url: string
  /** Host sends a response message, the messages "command" property will have this value. This allows service page to check this incoming "response" message. */
  responsecmd: string
}

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
