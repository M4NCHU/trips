export interface routerType {
  title: string;
  path: string;
  element: JSX.Element;
  isProtected: boolean;
  roles?: string[];
  isAdminPage?: boolean;
}
