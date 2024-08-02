export type InitMessage = {
  /** Informs page to do initialization based on currently loaded document for current company/user in host page */
  command: string
  /** URL of ERP host page (the page hosting the IFRAME) - to be used as message target by service page */
  messageTarget: string
  /** ID of the host page document (customer/supplier/supplier invoice, default is "0" if default value of the series is '<NY>'. Default is sent when creating new supplier/customer/supplier invoice.) */
  primaryKey: string
  /** Null for customer/supplier, document type for supplier invoice */
  secondaryKey: string
  /** Current company id */
  companyId: number
  /** Current user id (email) */
  user: string
  /** Current user's UI language */
  language: string
  /** Current user's tenant id */
  tenantId: string
  /** Current branch id */
  branchId: string
  /** Flag to make the integration aware if user has unsaved changes in current screen */
  userChangesPresent: boolean
}

export type GetResponse = {
  /** Command is the value that initially was sent in the message by the service page. Eg. page sends {"command":"apiget", "url":"api/v1/customer/abc", "responsecmd":"customerFetched"}
  and must listen for message like {"command":"customerFetched", "data":"<api response data>"} to handle the fetched customer data */
  command: string
  /** Data that came back from api get invocation */
  data: any
  /** Response status code if api call has failed */
  status: number
  /** Response error text if api call has failed */
  error: string
}

export type PostResponse = {
  /** Command is the value that initially was sent in the message by the service page. Eg. page sends {"command":"apipost", "url":"api/v1/customer", "content":"<customer data>", "responsecmd":"customerCreated"}
  and must listen for message like {"command":"customerCreated", "data":"<api response data>"} to handle the created customer data */
  command: string
  /** Data that came back from api post invocation */
  data: any
  /** Response status code if api call has failed */
  status: number
  /** Response error text if api call has failed */
  error: string
}

export type PutResponse = {
  /** Command is the value that initially was sent in the message by the service page. Eg. page sends {"command":"apiput", "url":"api/v1/customer/abc", "content":"<customer data>", "responsecmd":"customerUpdated"}
  and must listen for message like {"command":"customerUpdated", "data":"<api response data>"} to handle the updated customer data */
  command: string
  /** Data that came back from api put invocation */
  data: any
  /** Response status code if api call has failed */
  status: number
  /** Response error text if api call has failed */
  error: string
}

export type DeleteResponse = {
  /** Command is the value that initially was sent in the message by the service page. Eg. page sends {"command":"apidelete", "url":"api/v1/customer/abc", "responsecmd":"customerDeleted"}
   * and must listen for message like {"command":"customerDeleted", "data":"<api response data>"} to handle the deleted customer data */
  command: string
  /** Data that came back from api delete invocation */
  data: any
  /** Response status code if api call has failed */
  status: number
  /** Response error text if api call has failed */
  error: string
}

export type InitResponse = {
  /** Command is the value that initially was sent in the message by the service page. */
  responsecmd: string
  /** Response error text if api call has failed */
  error: string
}

export type ErrorResponse = {
  /** Command is the value that initially was sent in the message by the service page.
   * If either responsecmd was not set in request or the data sent from the request was missing/faulty, this value will be set to "error"
   */
  command: string
  /** Response data text if api call has failed */
  data: 'Unrecognized command'
}
