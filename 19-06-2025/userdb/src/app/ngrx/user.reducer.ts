import { createReducer, on } from "@ngrx/store";
import { initialUserState } from "./UserState";
import * as UserActions from "./user.actions";


export const userReducer = createReducer(initialUserState,
    on(UserActions.loadUsers, state => ({...state, loading:true, error:null})),
    on(UserActions.loadUsersFailure, (state, {error}) => ({...state, loading:false, error: error})),
    on(UserActions.loadUsersSuccess, (state, {users}) => ({...state, users:users, loading:false, error:null})),
    on(UserActions.addUser, (state, {user}) => ({...state, users: [...state.users, user], loading:false, error:null}))
)