import { createFeatureSelector, createSelector } from "@ngrx/store";
import { UserState } from "./UserState";


export const selectUserState = createFeatureSelector<UserState>('user')

export const selectAllUsers = createSelector(selectUserState, state => state.users)
export const selectIsLoading = createSelector(selectUserState, state => state.loading)
export const selectError = createSelector(selectUserState, state => state.error)